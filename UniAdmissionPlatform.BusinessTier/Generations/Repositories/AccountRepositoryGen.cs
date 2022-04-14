/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
namespace UniAdmissionPlatform.BusinessTier.Generations.Repositories
{
    public partial interface IAccountRepository :IBaseRepository<Account>
    {
    }
    public partial class AccountRepository :BaseRepository<Account>, IAccountRepository
    {
         public AccountRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

