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
    public partial interface IGoalAdmissionTypeRepository :IBaseRepository<GoalAdmissionType>
    {
    }
    public partial class GoalAdmissionTypeRepository :BaseRepository<GoalAdmissionType>, IGoalAdmissionTypeRepository
    {
         public GoalAdmissionTypeRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

