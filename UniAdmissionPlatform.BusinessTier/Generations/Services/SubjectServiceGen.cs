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
    
    public partial interface ISubjectService:IBaseService<Subject>
    {
    }
    public partial class SubjectService:BaseService<Subject>,ISubjectService
    {
        public SubjectService(IUnitOfWork unitOfWork,ISubjectRepository repository):base(unitOfWork,repository){}
    }
}
