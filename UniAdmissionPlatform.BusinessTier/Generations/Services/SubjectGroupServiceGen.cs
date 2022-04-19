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
    
    public partial interface ISubjectGroupService:IBaseService<SubjectGroup>
    {
    }
    public partial class SubjectGroupService:BaseService<SubjectGroup>,ISubjectGroupService
    {
        public SubjectGroupService(IUnitOfWork unitOfWork,ISubjectGroupRepository repository):base(unitOfWork,repository){}
    }
}
