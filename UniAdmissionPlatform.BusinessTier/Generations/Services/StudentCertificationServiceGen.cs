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
    
    public partial interface IStudentCertificationService:IBaseService<StudentCertification>
    {
    }
    public partial class StudentCertificationService:BaseService<StudentCertification>,IStudentCertificationService
    {
        public StudentCertificationService(IUnitOfWork unitOfWork,IStudentCertificationRepository repository):base(unitOfWork,repository){}
    }
}
