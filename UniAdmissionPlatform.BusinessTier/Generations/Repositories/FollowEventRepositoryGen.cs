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
    public partial interface IFollowEventRepository :IBaseRepository<FollowEvent>
    {
    }
    public partial class FollowEventRepository :BaseRepository<FollowEvent>, IFollowEventRepository
    {
         public FollowEventRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

