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
    
    public partial interface IMajorDepartmentService:IBaseService<MajorDepartment>
    {
    }
    public partial class MajorDepartmentService:BaseService<MajorDepartment>,IMajorDepartmentService
    {
        public MajorDepartmentService(IUnitOfWork unitOfWork,IMajorDepartmentRepository repository):base(unitOfWork,repository){}
    }
}
