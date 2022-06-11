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
    
    public partial interface IReasonService:IBaseService<Reason>
    {
    }
    public partial class ReasonService:BaseService<Reason>,IReasonService
    {
        public ReasonService(IUnitOfWork unitOfWork,IReasonRepository repository):base(unitOfWork,repository){}
    }
}
