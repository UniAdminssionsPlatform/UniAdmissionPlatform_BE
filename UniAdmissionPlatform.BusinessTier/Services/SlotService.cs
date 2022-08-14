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
        Task CreateSlots(int highSchoolId, List<CreateSlotRequest> createSlotsRequest);

        Task<PageResult<SlotWithEventsViewModel>> GetSlotForSchoolUni(int highSchoolId, SlotFilterForSchoolAdmin filter,
            bool isPaging, int page,
            int limit);

        Task<PageResult<SlotViewModel>> GetSlotForAdminUni(SlotFilterForUniAdmin filter,bool isPaging ,int page, int limit);

        Task<bool> CheckStatusOfSlot(int slotId, SlotStatus slotStatus);
        Task CloseSlot(int highSchoolId, int slotId);
        Task UpdateFullSlotStatus(int id, int highSchoolId = 0);
        Task UpdateSlot(int slotId, UpdateSlotRequest updateSlotRequest);
        Task<SlotWithEventsViewModel> GetEventsBySlotId(int id);
        Task OpenSlot(int slotId, int highSchoolId = 0);
    }

    public partial class SlotService
    {
        private readonly IConfigurationProvider _mapper;

        public SlotService(IUnitOfWork unitOfWork, ISlotRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task CreateSlots(int highSchoolId, List<CreateSlotRequest> createSlotsRequest)
        {
            var mapper = _mapper.CreateMapper();
            var slots = mapper.Map<List<Slot>>(createSlotsRequest);

            var slotsInDb = await Get().Where(s => s.EndDate >= DateTime.Now && s.HighSchoolId == highSchoolId)
                .ToListAsync();

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                if (slot.EndDate != null && slot.StartDate >= slot.EndDate)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest,
                        $"Slot thứ {i + 1} phải có thời gian kết thúc phải lớn hơn thời gian bắt đầu");
                }

                for (int j = 0; j < slots.Count; j++)
                {
                    if (j != i)
                    {
                        var s = slots[j];
                        if (s.StartDate <= slot.StartDate &&
                            s.EndDate >= slot.StartDate // start time nam trong thang slot khac
                            || slot.EndDate != null && s.StartDate <= slot.EndDate &&
                            s.EndDate >= slot.EndDate // end time nam trong thang slot khac
                            || slot.EndDate != null && s.StartDate <= slot.StartDate &&
                            s.EndDate >= slot.EndDate)
                        {
                            throw new ErrorResponse(StatusCodes.Status400BadRequest,
                                $"Slot thứ {i + 1} mà bạn tạo bị trùng lịch với các slot khác!");
                        }
                    }
                }


                if (slotsInDb.Any(s =>
                        s.StartDate <= slot.StartDate &&
                        s.EndDate > slot.StartDate // start time nam trong thang slot khac
                        || slot.EndDate != null && s.StartDate <= slot.EndDate &&
                        s.EndDate >= slot.EndDate // end time nam trong thang slot khac
                        || slot.EndDate != null && s.StartDate <= slot.StartDate &&
                        s.EndDate >= slot.EndDate))
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest,
                        $"Slot thứ {i + 1} mà bạn tạo bị trùng lịch với slot khác trong hệ thống!");
                }

                if (slot.StartDate == null || slot.StartDate.Value.Date < DateTime.Now.AddDays(7).Date)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest,
                        $"Slot thứ {i + 1} mà bạn tạo có thời gian bắt đầu lớn hơn 7 ngày so với hiện tại!");
                }

                slot.HighSchoolId = highSchoolId;
                slot.Status = (int) SlotStatus.Open;
            }

            await AddRangeAsyn(slots);
        }

        public async Task<int> CreateSlot(int highSchoolId, CreateSlotRequest createSlotRequest)
        {
            var mapper = _mapper.CreateMapper();
            var slot = mapper.Map<Slot>(createSlotRequest);

            if (slot.EndDate != null && slot.StartDate >= slot.EndDate)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
            }


            slot.HighSchoolId = highSchoolId;
            slot.Status = (int) SlotStatus.Open;

            await CreateAsyn(slot);
            return slot.Id;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SlotWithEventsViewModel>> GetSlotForSchoolUni(int highSchoolId,
            SlotFilterForSchoolAdmin filter, bool isPaging, int page, int limit)
        {
            var query = Get().Where(s => s.HighSchoolId == highSchoolId);

            if (filter.StartDate != null)
            {
                query = query.Where(s => s.StartDate >= filter.StartDate);
            }

            if (filter.EndDate != null)
            {
                query = query.Where(s => s.EndDate <= filter.EndDate);
            }

            if (filter.Status != null)
            {
                query = query.Where(s => s.Status == filter.Status);
            }

            if (isPaging)
            {
                var (total, queryable) = query.ProjectTo<SlotWithEventsViewModel>(_mapper)
                    .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
                return new PageResult<SlotWithEventsViewModel>
                {
                    List = await queryable.ToListAsync(),
                    Page = page == 0 ? 1 : page,
                    Limit = limit == 0 ? DefaultPaging : limit,
                    Total = total
                };
            }

            return new PageResult<SlotWithEventsViewModel>
            {
                List = await query.ProjectTo<SlotWithEventsViewModel>(_mapper).ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = query.Count(),
                Total = query.Count()
            };
        }

        public async Task<PageResult<SlotViewModel>> GetSlotForAdminUni(SlotFilterForUniAdmin filter, bool isPaging, int page,
            int limit)
        {
            var query = Get();

            if (filter.HighSchoolId != null)
            {
                query = query.Where(s => s.HighSchoolId == filter.HighSchoolId);
            }

            if (filter.StartDate != null)
            {
                query = query.Where(s => s.StartDate >= filter.StartDate);
            }

            if (filter.EndDate != null)
            {
                query = query.Where(s => s.EndDate <= filter.EndDate.Value.AddDays(1));
            }

            if (filter.Status != null)
            {
                query = query.Where(s => s.Status == filter.Status);
            }

            if (isPaging)
            {
                var (total, queryable) = query
                    .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
                return new PageResult<SlotViewModel>
                {
                    List = _mapper.CreateMapper().Map<List<SlotViewModel>>(await queryable.ToListAsync()),
                    Page = page == 0 ? 1 : page,
                    Limit = limit == 0 ? DefaultPaging : limit,
                    Total = total
                };
            }

            return new PageResult<SlotViewModel>
            {
                List = _mapper.CreateMapper().Map<List<SlotViewModel>>(await query.ToListAsync()),
                Page = page == 0 ? 1 : page,
                Limit = query.Count(),
                Total = query.Count()
            };
        }

        public async Task<bool> CheckStatusOfSlot(int slotId, SlotStatus slotStatus)
        {
            return await Get(s => s.Id == slotId && s.Status == (int) slotStatus).AnyAsync();
        }


        public async Task OpenSlot(int slotId, int highSchoolId = 0)
        {
            var slot = await Get(s => s.Id == slotId).FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    "Không tìm thấy buổi này");
            }
            
            if (highSchoolId != 0 && slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này không thuộc về bạn.");
            }

            if (slot.EndDate < DateTime.Now)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã kết thúc, không thể mở lại.");
            }

            slot.Status = (int)SlotStatus.Open;

            await UpdateAsyn(slot);
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

            if (slot.Status == (int) SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã bị đóng.");
            }

            if (slot.EventChecks.Any())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này còn chứa sự kiện.");
            }

            slot.Status = (int) SlotStatus.Close;

            await UpdateAsyn(slot);
        }

        public async Task UpdateFullSlotStatus(int id, int highSchoolId = 0)
        {
            var fullSlot = await Get().Where(s => s.Id == id).Include(s => s.EventChecks).FirstOrDefaultAsync();
            if (fullSlot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy slot với id = {id}");
            }
            
            
            if (highSchoolId == 0 || fullSlot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này không thuộc về bạn.");
            }
            
            if (!fullSlot.EventChecks.Any())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này không chứa bất kì event.");
            }

            if (fullSlot.Status == (int) SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Buổi này đã bị đóng.");
            }


            fullSlot.Status = (int) SlotStatus.Full;
            await UpdateAsyn(fullSlot);
        }

        public async Task UpdateSlot(int slotId, UpdateSlotRequest updateSlotRequest)
        {
            var slot = await Get().Where(s => s.Id == slotId).FirstOrDefaultAsync();
            
            if (slot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy slot với id = {slotId}");
            }

            if (slot.Status == (int) SlotStatus.Close)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Slot {slotId} của bạn đã đóng!");
            }
            
            // var checkDate = DateTime.Now.AddDays(7);
            // if (slot.StartDate <= checkDate)
            // {
            //     throw new ErrorResponse(StatusCodes.Status400BadRequest,
            //         "Ngày tạo slot phải lớn hơn ngày hôm nay 7 ngày!");
            // }
            
            if (slot.EndDate != null && slot.StartDate >= slot.EndDate)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Thời gian kết thúc slot phải lớn hơn thời gian bắt đầu slot");
            }

            var mapper = _mapper.CreateMapper();
            slot = mapper.Map(updateSlotRequest, slot);
            
            //todo: validate time

            await UpdateAsyn(slot);
        }

        public async Task<SlotWithEventsViewModel> GetEventsBySlotId(int id)
        {
            var slot = await Get().Where(s => s.Id == id).ProjectTo<SlotWithEventsViewModel>(_mapper)
                .FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không thể tìm thấy slot có id = {id}");
            }

            return slot;
        }
    }
}