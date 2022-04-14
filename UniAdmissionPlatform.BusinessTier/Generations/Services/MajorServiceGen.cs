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
    
    public partial interface IMajorService:IBaseService<Major>
    {
    }
    public partial class MajorService:BaseService<Major>,IMajorService
    {
        public MajorService(IUnitOfWork unitOfWork,IMajorRepository repository):base(unitOfWork,repository){}
    }
}
