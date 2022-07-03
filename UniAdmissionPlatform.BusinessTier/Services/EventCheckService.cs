using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IEventCheckService
    {
        Task ApproveEventToSlot(int highSchoolId, int eventCheckId);
        Task<EventBySlotBaseViewModel> GetEventBySlotId(int slotId, int highSchoolId);
        Task RejectEventToSlot(int highSchoolId, int eventCheckId, string reason);
        Task<PageResult<EventWithSlotViewModel>> GetEventsByHighSchoolId(int highSchoolId, string sort, int page,
            int limit);

        Task<PageResult<EventWithSlotViewModel>> GetEventsHistoryByHighSchoolId(int highSchoolId, string sort, int page,
            int limit);

        Task<PageResult<EventCheckWithEventAndSlotModel>> GetEventCheckForUniAdmin(int universityId, EventCheckWithEventAndSlotModel filter, string sort, int page, int limit);
        Task<PageResult<EventCheckWithEventAndSlotModel>> GetEventCheckForHighSchoolAdmin(int highSchoolId, EventCheckWithEventAndSlotModel filter, string sort, int page, int limit);
    }

    public partial class EventCheckService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IReasonRepository _reasonRepository;
        
        public EventCheckService(IUnitOfWork unitOfWork, IEventCheckRepository repository, IMapper mapper, IReasonRepository reasonRepository) : base(unitOfWork, 
            repository)
        {
            _reasonRepository = reasonRepository;
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task ApproveEventToSlot(int highSchoolId, int eventCheckId)
        {
            var eventCheck = await Get(ec => ec.Id == eventCheckId)
                .Include(ec => ec.Slot)
                .Include(ec => ec.Event)
                .FirstOrDefaultAsync();
            
            if (eventCheck.Slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này không phải của trường bạn.");
            }
            
            if (eventCheck == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy cuộc đặt lịch này.");
            }

            if (eventCheck.Status != (int)EventCheckStatus.Pending)
            {
                if (eventCheck.Status == (int)EventCheckStatus.Approved)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này đã được chấp nhận.");
                }
                
                if (eventCheck.Status == (int)EventCheckStatus.Rejected)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này đã bị từ chối.");
                }
            }

            var uniEvent = eventCheck.Event;
            if (uniEvent.DeletedAt != null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy sự kiện trong cuộc đặt lịch");
            }

            if (uniEvent.Status != (int) EventStatus.Init)
            {
                if (uniEvent.Status == (int) EventStatus.Cancel)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã bị hủy.");
                }
                
                if (uniEvent.Status == (int) EventStatus.Done)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã tổ chức rồi.");
                }
            }

            var slot = eventCheck.Slot;
            if (slot.Status != (int) SlotStatus.Open)
            {
                if (slot.Status == (int) SlotStatus.Close)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Buổi này đã bị đóng.");
                }
                
                if (slot.Status == (int) SlotStatus.Full)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Buổi này đã bị đầy lịch.");
                }
            }

            eventCheck.Status = (int)EventCheckStatus.Approved;
            eventCheck.UpdatedAt = DateTime.Now;
            await UpdateAsyn(eventCheck);

            BackgroundJob.Enqueue<IMailBookingService>(mailBookingService =>
                mailBookingService.SendEmailForApprovedEventToUniAdmin(eventCheck.Id));
        }
        
        public async Task RejectEventToSlot(int highSchoolId, int eventCheckId, string reason)
        {
            var eventCheck = await Get(ec => ec.Id == eventCheckId)
                .Include(ec => ec.Slot)
                .Include(ec => ec.Event).ThenInclude(e => e.UniversityEvents)
                .FirstOrDefaultAsync();
            
            if (eventCheck.Slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này không phải của trường bạn.");
            }
            
            if (eventCheck == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy cuộc đặt lịch này.");
            }

            if (eventCheck.Status != (int)EventCheckStatus.Pending)
            {
                if (eventCheck.Status == (int)EventCheckStatus.Rejected)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này đã bị từ chối.");
                }
            }

            var uniEvent = eventCheck.Event;
            if (uniEvent.DeletedAt != null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy sự kiện trong cuộc đặt lịch");
            }

            if (uniEvent.Status != (int) EventStatus.Init)
            {
                if (uniEvent.Status == (int) EventStatus.Cancel)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã bị hủy.");
                }
                
                if (uniEvent.Status == (int) EventStatus.Done)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã tổ chức rồi.");
                }
            }

            var slot = eventCheck.Slot;
            if (slot.Status != (int) SlotStatus.Open)
            {
                if (slot.Status == (int) SlotStatus.Close)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Buổi này đã bị đóng.");
                }
            }

            var reasonEntity = new Reason
            {
                ReferenceId = eventCheck.Id,
                UniversityId = eventCheck.Event.UniversityEvents.FirstOrDefault()?.UniversityId ?? null,
                HighSchoolId = eventCheck.Slot.HighSchoolId,
                Detail = reason,
                Type = (int?)ReasonEnum.HighSchoolRejectUniversity,
            };
            
            await _reasonRepository.CreateAsync(reasonEntity);
            await unitOfWork.CommitAsync();
            
            eventCheck.Status = (int)EventCheckStatus.Rejected;
            await UpdateAsyn(eventCheck);
        }
        
        public async Task<EventBySlotBaseViewModel> GetEventBySlotId(int slotId, int highSchoolId)
        {
            var eventBySlot = await Get().Where(ec => ec.SlotId == slotId &&  ec.DeletedAt == null)
                .Include(ec => ec.Event)
                .ProjectTo<EventBySlotBaseViewModel>(_mapper).FirstOrDefaultAsync();
            if (eventBySlot == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy sự kiện nào theo slot với slot id = {slotId}");
            }
            return eventBySlot;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;


        public async Task<PageResult<EventWithSlotViewModel>> GetEventsByHighSchoolId(int highSchoolId, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(ec => ec.Status != (int) EventCheckStatus.Rejected && ec.Slot.HighSchoolId == highSchoolId)
                .ProjectTo<EventWithSlotViewModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<EventWithSlotViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<EventWithSlotViewModel>> GetEventsHistoryByHighSchoolId(int highSchoolId, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(ec => ec.Status != (int) EventCheckStatus.Rejected && ec.Slot.HighSchoolId == highSchoolId  && ec.Slot.EndDate < DateTime.Now)
                .ProjectTo<EventWithSlotViewModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<EventWithSlotViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<PageResult<EventCheckWithEventAndSlotModel>> GetEventCheckForUniAdmin(int universityId, EventCheckWithEventAndSlotModel filter, string sort, int page, int limit)
        {
            var (total, queryable) =  Get().Where(ev => ev.DeletedAt == null && ev.Event.UniversityEvents.Select(ue => ue.UniversityId).Contains(universityId))
                .ProjectTo<EventCheckWithEventAndSlotModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
                
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<EventCheckWithEventAndSlotModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<PageResult<EventCheckWithEventAndSlotModel>> GetEventCheckForHighSchoolAdmin(int highSchoolId, EventCheckWithEventAndSlotModel filter, string sort, int page,
            int limit)
        {
            var (total, queryable) =  Get().Where(ev => ev.DeletedAt == null && ev.Slot.HighSchoolId == highSchoolId)
                .ProjectTo<EventCheckWithEventAndSlotModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
                
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<EventCheckWithEventAndSlotModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}