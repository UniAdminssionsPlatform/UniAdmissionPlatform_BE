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
    public partial interface IRoleRepository :IBaseRepository<Role>
    {
    }
    public partial class RoleRepository :BaseRepository<Role>, IRoleRepository
    {
         public RoleRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

