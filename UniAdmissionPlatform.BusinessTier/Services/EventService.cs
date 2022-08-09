using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal.Mappers;
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
        Task UpdateEvent(int id, int universityId, UpdateEventRequest updateEventRequest);
        Task DeleteEvent(int id, int universityId);
        Task<PageResult<EventWithSlotModel>> GetAllEvents(EventWithSlotModel filter, string sort,
            int page, int limit, int? userId);
        Task<EventWithSlotModel> GetEventByID(int Id, int? userId = 0);
        Task BookSlotForUniAdmin(int universityId, BookSlotForUniAdminRequest bookSlotForUniAdminRequest);

        Task<List<EventBaseViewModel>> GetListEventsForUniAdmin(int universityId, DateTime? fromDate, DateTime? toDate,
            string sort);

        Task CloseEvent();
        Task<List<EventByUniIdBaseViewModel>> GetListEventsByUniversityId(int universityId);

        Task<PageResult<ListEventByUniIdBaseViewModel>> GetListEventsByUniId(int universityId, string eventName,
            string eventHostName, int? eventTypeId, int? statusEvent, string sort, int page, int limit);
    }
    
    public partial class EventService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly IFollowEventRepository _followEventRepository;
        
        public EventService(IUnitOfWork unitOfWork, IEventRepository repository, IMapper mapper, IEventTypeRepository eventTypeRepository, IFollowEventRepository followEventRepository) : base(unitOfWork, 
            repository)
        {
            _eventTypeRepository = eventTypeRepository;
            _followEventRepository = followEventRepository;
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateEvent(int universityId, CreateEventRequest createEventRequest)
        {
            if (createEventRequest.EventTypeId == 1)
            {
                var errors = new StringBuilder();
                if (string.IsNullOrWhiteSpace(createEventRequest.MeetingUrl))
                {
                    errors.Append("Sự kiện online phải có link meet.").Append("/n");
                }

                if (errors.Length != 0)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, errors.ToString());
                }
            }
            
            var mapper = _mapper.CreateMapper();
            var uniEvent = mapper.Map<Event>(createEventRequest);
            uniEvent.UniversityId = universityId;

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
            
            
            uniEvent.Status = (int)EventStatus.Init;

            await CreateAsyn(uniEvent);
            return uniEvent.Id;
        }

        public async Task PublishEvent(int eventId, bool isPublish, int universityId = 0)
        {
            Event @event = null;
            if (universityId != 0)
            {
                @event = await Get().Include(e => e.EventChecks).Where(e => e.UniversityId == universityId && e.Id == eventId).FirstOrDefaultAsync();
                if (@event == null)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy sự kiện.");
                }
            }

            @event = await Get().Include(e => e.EventChecks).FirstOrDefaultAsync(e => e.Id == eventId);

            if (isPublish && @event.Status != (int) EventStatus.Init)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái sự kiện không hợp lệ.");
            }

            if (!isPublish && @event.Status != (int) EventStatus.OnGoing)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái sự kiện không hợp lệ.");
            }

            if (isPublish && @event.EventTypeId == 2 && !@event.EventChecks.Any())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Event này chưa được tổ chức tại bất kì trường nào");
            }

            if (isPublish)
            {
                @event.Status = (int)EventStatus.OnGoing;
            }
            else
            {
                @event.Status = (int)EventStatus.Init;
            }



            if (@event.Status == (int)EventStatus.OnGoing)
            {
                foreach (var eventEventCheck in @event.EventChecks.Where(ec => ec.Status == (int) EventCheckStatus.Approved))
                {
                    BackgroundJob.Enqueue<IMailBookingService>(mailBookingService =>
                        mailBookingService.SendEmailForApprovedEventToUniAdmin(eventEventCheck.Id));
                }                
            }
            

            await UpdateAsyn(@event);
        }


        public async Task UpdateEvent(int id, int universityId, UpdateEventRequest updateEventRequest)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null && t.UniversityId == universityId).FirstOrDefaultAsync();
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
        
        public async Task DeleteEvent(int id, int universityId)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null && t.UniversityId == universityId).FirstOrDefaultAsync();
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
        
        public async Task<PageResult<EventWithSlotModel>> GetAllEvents(EventWithSlotModel filter, string sort, int page, int limit, int? userId)
        {
            var (total, queryable) = Get().Where(t => t.DeletedAt == null).ProjectTo<EventWithSlotModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
        
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            var eventWithSlotModels = await queryable.ToListAsync();

            if (userId != null)
            {
                var followEvents = _followEventRepository.Get().Where(fe => eventWithSlotModels.Select(e => e.Id).Contains(fe.EventId) && fe.StudentId == userId).ToDictionary(fe => fe.EventId, fe => fe);
                foreach (var eventWithSlotModel in eventWithSlotModels)
                {
                    if (eventWithSlotModel.Id != null && followEvents.ContainsKey(eventWithSlotModel.Id.Value))
                    {
                        eventWithSlotModel.IsFollow = followEvents[eventWithSlotModel.Id.Value].Status ==
                                                      (int?)FollowEventStatus.Followed;
                    }
                }
            }
            
            return new PageResult<EventWithSlotModel>
            {
                List = eventWithSlotModels,
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<EventWithSlotModel> GetEventByID(int id, int? userId = 0)
        {
             var eventById = await Get().Where(e => e.Id == id && e.DeletedAt == null).ProjectTo<EventWithSlotModel>(_mapper).FirstOrDefaultAsync();
             
             if (eventById == null)
             {
                 throw new ErrorResponse(StatusCodes.Status400BadRequest,
                     $"Không tìm thấy event với id ={id}");
             }
             
             if (userId != null && userId != 0)
             {
                 var followEvent = await _followEventRepository.FirstOrDefaultAsync(fe => fe.StudentId == userId && fe.EventId == id);
                 if (followEvent != null)
                 {
                     eventById.IsFollow = followEvent.Status != null &&
                                          followEvent.Status == (int?)FollowEventStatus.Followed;
                 }
             }
             
             return eventById;
        }
        
        public async Task BookSlotForUniAdmin(int universityId, BookSlotForUniAdminRequest bookSlotForUniAdminRequest)
        {
            // check xem event do co phai cua university khong
            var uniEvent = await Get().Where(e =>
                    e.Id == bookSlotForUniAdminRequest.EventId && e.UniversityId == universityId)
                .Include(e => e.EventChecks)
                .ThenInclude(ec => ec.Slot)
                .FirstOrDefaultAsync();
            if (uniEvent.EventTypeId != 2)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Loại sự kiện không hợp lệ.");
            }
            
            if (uniEvent == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Event không tồn tại hoặc không thuộc trường của bạn.");
            }
            
            // neu event do da thuoc truong cua no roi thi check xem no da duoc book vao slot nay chua
            var ec = uniEvent.EventChecks.FirstOrDefault(ec => ec.SlotId == bookSlotForUniAdminRequest.SlotId);

             
            if (ec != null && ec.Status != (int) EventCheckStatus.Rejected)
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
                    e.UniversityId == universityId
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

        public async Task<List<EventByUniIdBaseViewModel>> GetListEventsByUniversityId(int universityId)
        {
            var events = await Get().Where(e => e.UniversityId == universityId && e.DeletedAt == null).ToListAsync();

            var eventBaseViewModels = _mapper.CreateMapper().Map<List<EventBaseViewModel>>(events);

            var eventByUniIdBaseViewModels = eventBaseViewModels.Select(e => new EventByUniIdBaseViewModel(e, e.UniversityId)).ToList();
            return eventByUniIdBaseViewModels;
        }

        public async Task<PageResult<ListEventByUniIdBaseViewModel>> GetListEventsByUniId(int universityId, string eventName, string eventHostName, int? eventTypeId, int? statusEvent,
            string sort, int page, int limit)
        {
            var events = Get().Include(e => e.University).AsQueryable();
            var total = 0;

            if (sort != null)
            {
                (total, events) = events
                    .Where(
                        e => e.DeletedAt == null
                             && (e.UniversityId == universityId)
                             && (eventName == null || e.Name.Contains(eventName))
                             && (eventHostName == null || e.HostName.Contains(eventHostName))
                             && (eventTypeId == null || e.EventTypeId == eventTypeId)
                             && (statusEvent == null || e.Status == statusEvent)
                    ).OrderBy(sort).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            }
            else
            {
                (total, events) = events
                    .Where(
                        e => e.DeletedAt == null
                             && (e.UniversityId == universityId)
                             && (eventName == null || e.Name.Contains(eventName))
                             && (eventHostName == null || e.HostName.Contains(eventHostName))
                             && (eventTypeId == null || e.EventTypeId == eventTypeId)
                             && (statusEvent == null || e.Status == statusEvent)
                    ).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            }
            
            

            
            var eventWithIsApproveModels = _mapper.CreateMapper().Map<List<EventWithIsApproveModel>>(await events.ToListAsync()).Select(e => new ListEventByUniIdBaseViewModel(e, e.UniversityId)).ToList();

            return new PageResult<ListEventByUniIdBaseViewModel>
            {
                Limit = limit,
                List = eventWithIsApproveModels,
                Page = page,
                Total = total
            };
        }
    }
}