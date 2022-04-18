using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
                throw new ErrorResponse((int)HttpStatusCode.BadRequest,
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
            }
            
            if (await Get().Where(s => s.StartTime <= slot.StartTime && s.EndTime >= slot.StartTime // start time nam trong thang slot khac
                                      || slot.EndTime != null && s.StartTime <= slot.EndTime && s.EndTime >= slot.EndTime // end time nam trong thang slot khac
                                      || slot.EndTime != null && s.StartTime <= slot.StartTime && s.EndTime >= slot.EndTime // ca slot lan end time deu nam trong slot khac
                                      ).AnyAsync())
            {
                throw new ErrorResponse((int)HttpStatusCode.BadRequest,
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
    }
}