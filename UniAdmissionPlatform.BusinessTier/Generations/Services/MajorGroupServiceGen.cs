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
    
    public partial interface IMajorGroupService:IBaseService<MajorGroup>
    {
    }
    public partial class MajorGroupService:BaseService<MajorGroup>,IMajorGroupService
    {
        public MajorGroupService(IUnitOfWork unitOfWork,IMajorGroupRepository repository):base(unitOfWork,repository){}
    }
}
