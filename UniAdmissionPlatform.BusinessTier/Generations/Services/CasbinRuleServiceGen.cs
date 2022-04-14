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
    
    public partial interface ICasbinRuleService:IBaseService<CasbinRule>
    {
    }
    public partial class CasbinRuleService:BaseService<CasbinRule>,ICasbinRuleService
    {
        public CasbinRuleService(IUnitOfWork unitOfWork,ICasbinRuleRepository repository):base(unitOfWork,repository){}
    }
}
