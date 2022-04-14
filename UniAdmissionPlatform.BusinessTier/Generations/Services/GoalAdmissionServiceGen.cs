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
    
    public partial interface IGoalAdmissionService:IBaseService<GoalAdmission>
    {
    }
    public partial class GoalAdmissionService:BaseService<GoalAdmission>,IGoalAdmissionService
    {
        public GoalAdmissionService(IUnitOfWork unitOfWork,IGoalAdmissionRepository repository):base(unitOfWork,repository){}
    }
}
