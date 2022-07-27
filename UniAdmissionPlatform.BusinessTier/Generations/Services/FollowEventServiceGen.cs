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
    
    public partial interface IFollowEventService:IBaseService<FollowEvent>
    {
    }
    public partial class FollowEventService:BaseService<FollowEvent>,IFollowEventService
    {
        public FollowEventService(IUnitOfWork unitOfWork,IFollowEventRepository repository):base(unitOfWork,repository){}
    }
}
