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
    
    public partial interface IDistrictService:IBaseService<District>
    {
    }
    public partial class DistrictService:BaseService<District>,IDistrictService
    {
        public DistrictService(IUnitOfWork unitOfWork,IDistrictRepository repository):base(unitOfWork,repository){}
    }
}
