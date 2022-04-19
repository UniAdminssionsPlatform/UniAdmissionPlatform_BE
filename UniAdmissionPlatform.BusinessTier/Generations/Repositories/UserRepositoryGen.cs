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
    public partial interface IUserRepository :IBaseRepository<User>
    {
    }
    public partial class UserRepository :BaseRepository<User>, IUserRepository
    {
         public UserRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

