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
    
    public partial interface ICertificationService:IBaseService<Certification>
    {
    }
    public partial class CertificationService:BaseService<Certification>,ICertificationService
    {
        public CertificationService(IUnitOfWork unitOfWork,ICertificationRepository repository):base(unitOfWork,repository){}
    }
}
