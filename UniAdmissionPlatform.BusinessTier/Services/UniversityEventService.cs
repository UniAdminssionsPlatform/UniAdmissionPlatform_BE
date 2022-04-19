
using System;
using System.Threading.Tasks;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;


namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityEventService
    {
        Task CreateUniversityEvent(int universityId, int eventId);
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
            try
            {
                var uniEvent = new UniversityEvent{UniversityId = universityId , EventId = eventId};
                await CreateAsyn(uniEvent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }    

        }
    }
}