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
using Microsoft.Extensions.Logging;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IEventService
    {
        Task<int> CreateEvent(int universityId, CreateEventRequest createEventRequest);
        Task PublishEvent(int eventId, bool isPublish, int universityId = 0);
        Task UpdateEvent(int id, UpdateEventRequest updateEventRequest);
        Task DeleteEvent(int id);
        Task<PageResult<EventWithSlotModel>> GetAllEvents(EventWithSlotModel filter, string sort,
            int page, int limit);
        Task<EventBaseViewModel> GetEventByID(int Id);
        Task BookSlotForUniAdmin(int universityId, BookSlotForUniAdminRequest bookSlotForUniAdminRequest);

        Task<List<EventBaseViewModel>> GetListEventsForUniAdmin(int universityId, DateTime? fromDate, DateTime? toDate,
            string sort);

        Task CloseEvent();
    }
    
    public partial class EventService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IEventTypeRepository _eventTypeRepository;
        
        public EventService(IUnitOfWork unitOfWork, IEventRepository repository, IMapper mapper, IEventTypeRepository eventTypeRepository) : base(unitOfWork, 
            repository)
        {
            _eventTypeRepository = eventTypeRepository;
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateEvent(int universityId, CreateEventRequest createEventRequest)
        {
            
            var mapper = _mapper.CreateMapper();
            var uniEvent = mapper.Map<Event>(createEventRequest);

            var uniEventUniversityEvents = uniEvent.UniversityEvents = new List<UniversityEvent>();
            uniEventUniversityEvents.Add(new UniversityEvent
            {
                UniversityId = universityId,
            });

            // var checkDate = DateTime.Now.AddDays(7);
            // if (uniEvent.StartTime <= checkDate)
            // {
            //     throw new ErrorResponse(StatusCodes.Status400BadRequest,
            //         "Ngày diễn ra sự kiện phải lớn hơn ngày hôm nay 7 ngày!");
            // }
            
            if (uniEvent.StartTime != null && uniEvent.EndTime != null && uniEvent.StartTime >= uniEvent.EndTime)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Ngày kết thúc sự kiện phải lớn hơn ngày diễn ra sự kiện!");
            }

            if (uniEvent.EventTypeId == 2)
            {
                
            }
            
            
            uniEvent.Status = (int)EventStatus.Init;

            await CreateAsyn(uniEvent);
            return uniEvent.Id;
        }

        public async Task PublishEvent(int eventId, bool isPublish, int universityId = 0)
        {
            Event @event = null;
            if (universityId != 0)
            {
                @event = await Get().Where(e => e.UniversityEvents.Select(ue => ue.UniversityId).Contains(universityId) && e.Id == eventId).FirstOrDefaultAsync();
                if (@event == null)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy sự kiện.");
                }
            }

            @event = await FirstOrDefaultAsyn(e => e.Id == eventId);

            if (isPublish && @event.Status != (int) EventStatus.Init)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái sự kiện không hợp lệ.");
            }

            if (!isPublish && @event.Status != (int) EventStatus.OnGoing)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái sự kiện không hợp lệ.");
            }

            if (isPublish && @event.EventTypeId != 2)
            {
                
            }

            @event.Status = (int)EventStatus.OnGoing;

            await UpdateAsyn(@event);
        }


        public async Task UpdateEvent(int id, UpdateEventRequest updateEventRequest)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null).FirstOrDefaultAsync();
            if (uniEvent == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy event với id = {id}");
            }
            
            var mapper = _mapper.CreateMapper();
            uniEvent = mapper.Map(updateEventRequest,uniEvent);
            
            
            if (uniEvent.StartTime >= uniEvent.EndTime)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Ngày kết thúc sự kiện phải lớn hơn ngày diễn ra sự kiện!");
            }
            
            uniEvent.UpdatedAt = DateTime.Now;
            await UpdateAsyn(uniEvent);
        }
        
        public async Task DeleteEvent(int id)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null).FirstOrDefaultAsync();
            if (uniEvent == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy event với id = {id}");
            }
            
            uniEvent.DeletedAt = DateTime.Now;
            uniEvent.Status = (int) EventStatus.Cancel;
            await UpdateAsyn(uniEvent);
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;
        
        public async Task<PageResult<EventWithSlotModel>> GetAllEvents(EventWithSlotModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(t => t.DeletedAt == null).ProjectTo<EventWithSlotModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
        
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<EventWithSlotModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<EventBaseViewModel> GetEventByID(int id)
        {
             var eventById = await Get().Where(e => e.Id == id && e.DeletedAt == null).ProjectTo<EventBaseViewModel>(_mapper).FirstOrDefaultAsync();
             if (eventById == null)
             {
                 throw new ErrorResponse(StatusCodes.Status400BadRequest,
                     $"Không tìm thấy event với id ={id}");
             }
             return eventById;
        }
        
        public async Task BookSlotForUniAdmin(int universityId, BookSlotForUniAdminRequest bookSlotForUniAdminRequest)
        {
            // check xem event do co phai cua university khong
            var uniEvent = await Get().Where(e =>
                    e.Id == bookSlotForUniAdminRequest.EventId && e.UniversityEvents.Contains(new UniversityEvent {UniversityId = universityId, EventId = bookSlotForUniAdminRequest.EventId}))
                .Include(e => e.EventChecks)
                .ThenInclude(ec => ec.Slot)
                .FirstOrDefaultAsync();
            if (uniEvent == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Event không tồn tại hoặc không thuộc trường của bạn.");
            }
            
            // neu event do da thuoc truong cua no roi thi check xem no da duoc book vao slot nay chua
            var ec = uniEvent.EventChecks.FirstOrDefault(ec => ec.SlotId == bookSlotForUniAdminRequest.SlotId);

             
            if (ec != null && ec.Status != (int) EventCheckStatus.Reject)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Event của bạn đã được book ở slot này.");
            }

            var eventCheck = new EventCheck
            {
                SlotId = bookSlotForUniAdminRequest.SlotId,
                EventId = bookSlotForUniAdminRequest.EventId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = (int) EventCheckStatus.Pending,
            };

            uniEvent.EventChecks.Add(eventCheck);

            await UpdateAsyn(uniEvent);

            BackgroundJob.Enqueue<IMailBookingService>(mailBookingService =>
                mailBookingService.SendMailForNewBookingToSchoolAdmin(eventCheck.Id));
        }

        public async Task<List<EventBaseViewModel>> GetListEventsForUniAdmin(int universityId, DateTime? fromDate, DateTime? toDate, string sort)
        {
            var queryable = Get().Where(
                e =>
                    e.DeletedAt == null &&
                    e.UniversityEvents.Select(ue => ue.UniversityId).Contains(universityId)
                     && (fromDate == null || e.StartTime >= fromDate)
                         && (toDate == null || e.EndTime == null || e.EndTime <= toDate)
                     );

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return await queryable.ProjectTo<EventBaseViewModel>(_mapper).ToListAsync();
        }

        public async Task CloseEvent()
        {
            var eventType = await _eventTypeRepository.FirstOrDefaultAsync(et => et.Name == "Offline In High School");
            if (eventType == null)
            {
                throw new ErrorResponse(StatusCodes.Status500InternalServerError, "Server error");
            }

            var events = Get().Where(e => e.EndTime != null && e.EndTime < DateTime.Now).ToList();
            foreach (var @event in events)
            {
                @event.Status = 2;
            }

            await SaveAsyn();
        }
    }
}