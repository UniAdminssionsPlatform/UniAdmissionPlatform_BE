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
    public partial interface IGoalAdmissionRepository :IBaseRepository<GoalAdmission>
    {
    }
    public partial class GoalAdmissionRepository :BaseRepository<GoalAdmission>, IGoalAdmissionRepository
    {
         public GoalAdmissionRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

