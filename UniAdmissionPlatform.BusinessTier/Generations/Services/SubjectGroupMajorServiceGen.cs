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
    
    public partial interface ISubjectGroupMajorService:IBaseService<SubjectGroupMajor>
    {
    }
    public partial class SubjectGroupMajorService:BaseService<SubjectGroupMajor>,ISubjectGroupMajorService
    {
        public SubjectGroupMajorService(IUnitOfWork unitOfWork,ISubjectGroupMajorRepository repository):base(unitOfWork,repository){}
    }
}
