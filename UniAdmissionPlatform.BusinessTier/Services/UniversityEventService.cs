
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
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
                    $"Không tìm thấy sự kiện nào theo university với id ={eventByUni}");
            }
            return eventByUni;
        }
    }
}