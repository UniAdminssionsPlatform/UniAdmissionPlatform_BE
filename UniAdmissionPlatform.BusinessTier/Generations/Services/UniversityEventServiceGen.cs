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
    
    public partial interface IUniversityEventService:IBaseService<UniversityEvent>
    {
    }
    public partial class UniversityEventService:BaseService<UniversityEvent>,IUniversityEventService
    {
        public UniversityEventService(IUnitOfWork unitOfWork,IUniversityEventRepository repository):base(unitOfWork,repository){}
    }
}
