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
    
    public partial interface IRoleService:IBaseService<Role>
    {
    }
    public partial class RoleService:BaseService<Role>,IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork,IRoleRepository repository):base(unitOfWork,repository){}
    }
}
