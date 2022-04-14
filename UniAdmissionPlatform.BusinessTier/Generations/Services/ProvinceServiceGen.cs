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
    
    public partial interface IProvinceService:IBaseService<Province>
    {
    }
    public partial class ProvinceService:BaseService<Province>,IProvinceService
    {
        public ProvinceService(IUnitOfWork unitOfWork,IProvinceRepository repository):base(unitOfWork,repository){}
    }
}
