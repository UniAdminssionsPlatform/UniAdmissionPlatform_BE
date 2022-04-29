using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISlotService
    {
        Task<int> CreateSlot(int highSchoolId, CreateSlotRequest createSlotRequest);
        Task<PageResult<SlotViewModel>> GetSlotForSchoolUni(int highSchoolId, SlotFilterForSchoolAdmin filter, int page, int limit);
        Task<PageResult<SlotViewModel>> GetSlotForAdminUni(SlotFilterForUniAdmin filter, int page, int limit);

        Task<bool> CheckStatusOfSlot(int slotId, SlotStatus slotStatus);
        Task CloseSlot(int highSchoolId, int slotId);
        Task UpdateFullSlotStatus(int id);
        Task UpdateSlot(int slotId, UpdateSlotRequest updateSlotRequest);

    }
    
    public partial class SlotService
    {
        private readonly IConfigurationProvider _mapper;

        public SlotService(IUnitOfWork unitOfWork, ISlotRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateSlot(int highSchoolId, CreateSlotRequest createSlotRequest)
        {
            var mapper = _mapper.CreateMapper();
            var slot = mapper.Map<Slot>(createSlotRequest);

            if (slot.EndTime != null && slot.StartTime >= slot.EndTime)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
            }
            
            if (await Get().Where(s => s.StartTime <= slot.StartTime && s.EndTime >= slot.StartTime // start time nam trong thang slot khac
                                      || slot.EndTime != null && s.StartTime <= slot.EndTime && s.EndTime >= slot.EndTime // end time nam trong thang slot khac
                                      || slot.EndTime != null && s.StartTime <= slot.StartTime && s.EndTime >= slot.EndTime // ca slot lan end time deu nam trong slot khac
                                      ).AnyAsync())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Slot bị trùng lịch với slot khác!");
            }
            
            
            slot.HighSchoolId = highSchoolId;
            slot.Status = (int)SlotStatus.Open;

            await CreateAsyn(slot);
            return slot.Id;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;
        public async Task<PageResult<SlotViewModel>> GetSlotForSchoolUni(int highSchoolId, SlotFilterForSchoolAdmin filter, int page, int limit)
        {
            var query = Get().Where(s => s.HighSchoolId == highSchoolId);

            if (filter.StartTime != null)
            {
                query = query.Where(s => s.StartTime >= filter.StartTime);
            }

            if (filter.EndTime != null)
            {
                query = query.Where(s => s.EndTime <= filter.EndTime);
            }

            if (filter.Status != null)
            {
                query = query.Where(s => s.Status == filter.Status);
            }

            var (total, queryable) = query.ProjectTo<SlotViewModel>(_mapper).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            return new PageResult<SlotViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<SlotViewModel>> GetSlotForAdminUni(SlotFilterForUniAdmin filter, int page, int limit)
        {
            var query = Get();

            if (filter.HighSchoolId != null)
            {
                query = query.Where(s => s.HighSchoolId == filter.HighSchoolId);
            }

            if (filter.StartTime != null)
            {
                query = query.Where(s => s.StartTime >= filter.StartTime);
            }

            if (filter.EndTime != null)
            {
                query = query.Where(s => s.EndTime <= filter.EndTime);
            }

            if (filter.Status != null)
            {
                query = query.Where(s => s.Status == filter.Status);
            }

            var (total, queryable) = query.ProjectTo<SlotViewModel>(_mapper).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            return new PageResult<SlotViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<bool> CheckStatusOfSlot(int slotId, SlotStatus slotStatus)
        {
            return await Get(s => s.Id == slotId && s.Status == (int) slotStatus).AnyAsync();
        }

        public async Task CloseSlot(int highSchoolId, int slotId)
        {
            var slot = await Get(s => s.Id == slotId).Include(s => s.EventChecks).FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    "Không tìm thấy buổi này");
            }

            if (slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này không thuộc về bạn.");
                
            }

            if (slot.Status == (int)SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã bị đóng.");
            }

            slot.Status = (int)SlotStatus.Close;
            foreach (var eventCheck in slot.EventChecks)
            {
                eventCheck.Status = (int)EventCheckStatus.Reject;
                //todo: send notification to university
            }
            
            await UpdateAsyn(slot);
        }
        
        public async Task UpdateFullSlotStatus(int id)
        {
            var fullSlot = await Get().Where(s => s.Id == id).FirstOrDefaultAsync();
            if (fullSlot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy slot với id = {id}");
            }
            
            if (fullSlot.Status == (int)SlotStatus.Full)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã bị đầy.");
            }
            
            if (fullSlot.Status == (int)SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã bị đóng.");
            }

            fullSlot.Status = (int)SlotStatus.Full;
            await UpdateAsyn(fullSlot);
        }
        
        public async Task UpdateSlot(int slotId, UpdateSlotRequest updateSlotRequest)
        {
            var slot = await Get().Where(s => s.Id == slotId).FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy slot với id = {slotId}");
            }

            if (slot.Status == (int)SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Slot {slotId} của bạn đã đóng!");
            }
            
            if (slot.Status == (int)SlotStatus.Full)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Slot {slotId} của bạn đã đầy!");
            }

            var mapper = _mapper.CreateMapper();
            slot = mapper.Map(updateSlotRequest,slot);
            
            await UpdateAsyn(slot);
        }
    }
}