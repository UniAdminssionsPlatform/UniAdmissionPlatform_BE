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
    public partial interface IGroupPointRepository :IBaseRepository<GroupPoint>
    {
    }
    public partial class GroupPointRepository :BaseRepository<GroupPoint>, IGroupPointRepository
    {
         public GroupPointRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

