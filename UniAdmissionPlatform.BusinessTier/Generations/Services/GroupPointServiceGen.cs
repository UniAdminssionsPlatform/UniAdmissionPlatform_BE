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
    
    public partial interface IGroupPointService:IBaseService<GroupPoint>
    {
    }
    public partial class GroupPointService:BaseService<GroupPoint>,IGroupPointService
    {
        public GroupPointService(IUnitOfWork unitOfWork,IGroupPointRepository repository):base(unitOfWork,repository){}
    }
}
