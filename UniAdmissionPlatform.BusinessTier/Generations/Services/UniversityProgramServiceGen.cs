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
    
    public partial interface IUniversityProgramService:IBaseService<UniversityProgram>
    {
    }
    public partial class UniversityProgramService:BaseService<UniversityProgram>,IUniversityProgramService
    {
        public UniversityProgramService(IUnitOfWork unitOfWork,IUniversityProgramRepository repository):base(unitOfWork,repository){}
    }
}
