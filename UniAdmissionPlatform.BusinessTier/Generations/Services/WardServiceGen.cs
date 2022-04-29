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
    
    public partial interface IWardService:IBaseService<Ward>
    {
    }
    public partial class WardService:BaseService<Ward>,IWardService
    {
        public WardService(IUnitOfWork unitOfWork,IWardRepository repository):base(unitOfWork,repository){}
    }
}
