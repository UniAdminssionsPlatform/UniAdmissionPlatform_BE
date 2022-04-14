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
    
    public partial interface IGoalAdmissionTypeService:IBaseService<GoalAdmissionType>
    {
    }
    public partial class GoalAdmissionTypeService:BaseService<GoalAdmissionType>,IGoalAdmissionTypeService
    {
        public GoalAdmissionTypeService(IUnitOfWork unitOfWork,IGoalAdmissionTypeRepository repository):base(unitOfWork,repository){}
    }
}
