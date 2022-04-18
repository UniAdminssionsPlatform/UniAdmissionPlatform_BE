using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IEventService
    {
        Task<int> CreateEvent(CreateEventRequest createEventRequest);
        Task UpdateEvent(int id, UpdateEventRequest updateEventRequest);
        Task DeleteEvent(int id);
        Task<PageResult<EventBaseViewModel>> GetAllEvents(EventBaseViewModel filter, string sort,
            int page, int limit);
    }
    
    public partial class EventService
    {
        private readonly IConfigurationProvider _mapper;
        
        public EventService(IUnitOfWork unitOfWork, IEventRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateEvent(CreateEventRequest createEventRequest)
        {
            
            var mapper = _mapper.CreateMapper();
            var uniEvent = mapper.Map<Event>(createEventRequest);
            
            await CreateAsyn(uniEvent);
            return uniEvent.Id;
        }


        public async Task UpdateEvent(int id, UpdateEventRequest updateEventRequest)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null).FirstOrDefaultAsync();
            if (uniEvent == null)
            {
                throw new ErrorResponse((int) HttpStatusCode.NotFound, $"Không tìm thấy event với id = {id}");
            }
        
            var mapper = _mapper.CreateMapper();
            var eventInRequest = mapper.Map<Event>(updateEventRequest);
        
            uniEvent.Name = eventInRequest.Name;
            uniEvent.UpdatedAt = DateTime.Now;
        
            await UpdateAsyn(uniEvent);
        }
        
        public async Task DeleteEvent(int id)
        {
            var uniEvent = await Get().Where(t => t.Id == id && t.DeletedAt == null).FirstOrDefaultAsync();
            if (uniEvent == null)
            {
                throw new ErrorResponse((int) HttpStatusCode.NotFound, $"Không tìm thấy event với id = {id}");
            }
            
            uniEvent.DeletedAt = DateTime.Now;
            
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
    }
}