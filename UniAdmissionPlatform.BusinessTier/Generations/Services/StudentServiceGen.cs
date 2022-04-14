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
    
    public partial interface IStudentService:IBaseService<Student>
    {
    }
    public partial class StudentService:BaseService<Student>,IStudentService
    {
        public StudentService(IUnitOfWork unitOfWork,IStudentRepository repository):base(unitOfWork,repository){}
    }
}
