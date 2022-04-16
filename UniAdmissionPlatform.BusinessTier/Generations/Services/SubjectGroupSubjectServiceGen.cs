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
    
    public partial interface ISubjectGroupSubjectService:IBaseService<SubjectGroupSubject>
    {
    }
    public partial class SubjectGroupSubjectService:BaseService<SubjectGroupSubject>,ISubjectGroupSubjectService
    {
        public SubjectGroupSubjectService(IUnitOfWork unitOfWork,ISubjectGroupSubjectRepository repository):base(unitOfWork,repository){}
    }
}
