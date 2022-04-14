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
    
    public partial interface IAccountService:IBaseService<Account>
    {
    }
    public partial class AccountService:BaseService<Account>,IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork,IAccountRepository repository):base(unitOfWork,repository){}
    }
}
