/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    
    public partial interface IEventCheckService:IBaseService<EventCheck>
    {
    }
    public partial class EventCheckService:BaseService<EventCheck>,IEventCheckService
    {
        public EventCheckService(IUnitOfWork unitOfWork,IEventCheckRepository repository, IReasonRepository reasonRepository):base(unitOfWork,repository)
        {
            _reasonRepository = reasonRepository;
        }
    }
}
