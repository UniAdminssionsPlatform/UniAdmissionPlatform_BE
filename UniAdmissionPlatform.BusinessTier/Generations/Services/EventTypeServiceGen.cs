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
    
    public partial interface IEventTypeService:IBaseService<EventType>
    {
    }
    public partial class EventTypeService:BaseService<EventType>,IEventTypeService
    {
        public EventTypeService(IUnitOfWork unitOfWork,IEventTypeRepository repository):base(unitOfWork,repository){}
    }
}
