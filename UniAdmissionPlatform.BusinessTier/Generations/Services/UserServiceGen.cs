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
    
    public partial interface IUserService:IBaseService<User>
    {
    }
    public partial class UserService:BaseService<User>,IUserService
    {
        public UserService(IUnitOfWork unitOfWork,IUserRepository repository):base(unitOfWork,repository){}
    }
}
