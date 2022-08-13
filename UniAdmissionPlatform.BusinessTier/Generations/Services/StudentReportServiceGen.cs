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
    
    public partial interface IStudentReportService:IBaseService<StudentReport>
    {
    }
    public partial class StudentReportService:BaseService<StudentReport>,IStudentReportService
    {
        public StudentReportService(IUnitOfWork unitOfWork,IStudentReportRepository repository):base(unitOfWork,repository){}
    }
}
