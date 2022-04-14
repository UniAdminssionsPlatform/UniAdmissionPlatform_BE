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
    
    public partial interface IRegionService:IBaseService<Region>
    {
    }
    public partial class RegionService:BaseService<Region>,IRegionService
    {
        public RegionService(IUnitOfWork unitOfWork,IRegionRepository repository):base(unitOfWork,repository){}
    }
}
