﻿using System;
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
        Task UpdateEvent(int id, UpdateEventRequest updateEventRequest);
        Task DeleteEvent(int id);
        Task<PageResult<EventBaseViewModel>> GetAllEvents(EventBaseViewModel filter, string sort,
            int page, int limit);
        Task<EventBaseViewModel> GetEventByID(int Id);
        Task BookSlotForUniAdmin(int universityId, BookSlotForUniAdminRequest bookSlotForUniAdminRequest);

        Task<List<EventBaseViewModel>> GetListEventsForUniAdmin(int universityId, DateTime? fromDate, DateTime? toDate,
            string sort);
    }
    
    public partial class EventService
    {
        private readonly IConfigurationProvider _mapper;
        
        public EventService(IUnitOfWork unitOfWork, IEventRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
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

            var checkDate = DateTime.Now.AddDays(7);
            if (uniEvent.StartTime <= checkDate)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Ngày diễn ra sự kiện phải lớn hơn ngày hôm nay 7 ngày!");
            }
            
            if (uniEvent.StartTime >= uniEvent.EndTime)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Ngày kết thúc sự kiện phải lớn hơn ngày diễn ra sự kiện!");
            }
            
            uniEvent.Status = (int) EventStatus.OnGoing;
            await CreateAsyn(uniEvent);
            return uniEvent.Id;
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
            
            var checkDate = DateTime.Now.AddDays(7);
            if (uniEvent.StartTime <= checkDate)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Ngày diễn ra sự kiện phải lớn hơn ngày hôm nay 7 ngày!");
            }
            
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
        
        public async Task<PageResult<EventBaseViewModel>> GetAllEvents(EventBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(t => t.DeletedAt == null).ProjectTo<EventBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
        
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<EventBaseViewModel>
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
    }
}