using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
namespace UniAdmissionPlatform.BusinessTier.Generations.Repositories
{
    public partial interface IFollowRepository :IBaseRepository<Follow>
    {
    }
    public partial class FollowRepository :BaseRepository<Follow>, IFollowRepository
    {
        public FollowRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}