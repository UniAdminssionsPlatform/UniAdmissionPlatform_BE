
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;


namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityEventService
    {
        Task CreateUniversityEvent(int universityId, int eventId);
        Task<EventByUniIdBaseViewModel> GetEventByUniId(int universityId);

        Task<PageResult<ListEventByUniIdBaseViewModel>> GetListEventsByUniId(int universityId,string eventName, string eventHostName,int? eventTypeId, int? statusEvent, string sort, int page, int limit);

        Task<PageResult<ListEventByUniIdBaseViewModel>> GetListOnGoingEventsByUniId(int universityId, string sort,
            int page, int limit);
    }

    public partial class UniversityEventService
    {
        private readonly IConfigurationProvider _mapper;
        
        public UniversityEventService(IUnitOfWork unitOfWork, IUniversityEventRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task CreateUniversityEvent(int universityId, int eventId)
        {
            var uniEvent = new UniversityEvent{UniversityId = universityId , EventId = eventId};
            await CreateAsyn(uniEvent);
        }
        
        public async Task<EventByUniIdBaseViewModel> GetEventByUniId(int universityId)
        {
            var eventByUni = await Get().Where(ue => ue.UniversityId == universityId)
                .Include(ue => ue.Event)
                .ProjectTo<EventByUniIdBaseViewModel>(_mapper).FirstOrDefaultAsync();
            if (eventByUni == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy sự kiện nào theo university với university id ={universityId}");
            }
            return eventByUni;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;
        public async Task<PageResult<ListEventByUniIdBaseViewModel>> GetListEventsByUniId(int universityId,string eventName, string eventHostName, int? eventTypeId, int? statusEvent, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(ue => ue.UniversityId == universityId
                                                       && (eventName == null || ue.Event.Name.Contains(eventName)) 
                                                       && (eventHostName == null || ue.Event.HostName.Contains(eventHostName))
                                                       && (eventTypeId == null || ue.Event.EventTypeId == eventTypeId)
                                                       && (statusEvent == null || ue.Event.Status == statusEvent))
                .ProjectTo<ListEventByUniIdBaseViewModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<ListEventByUniIdBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<ListEventByUniIdBaseViewModel>> GetListOnGoingEventsByUniId(int universityId, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(ue => ue.UniversityId == universityId
                                                       && (ue.Event.Status == (int) EventStatus.OnGoing))
                .ProjectTo<ListEventByUniIdBaseViewModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<ListEventByUniIdBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}