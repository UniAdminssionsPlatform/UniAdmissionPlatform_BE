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
    
    public partial interface IEventService:IBaseService<Event>
    {
    }
    public partial class EventService:BaseService<Event>,IEventService
    {
        public EventService(IUnitOfWork unitOfWork,IEventRepository repository):base(unitOfWork,repository){}
    }
}
