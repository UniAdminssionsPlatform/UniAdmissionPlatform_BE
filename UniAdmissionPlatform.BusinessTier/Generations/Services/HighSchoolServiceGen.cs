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
    
    public partial interface IHighSchoolService:IBaseService<HighSchool>
    {
    }
    public partial class HighSchoolService:BaseService<HighSchool>,IHighSchoolService
    {
        public HighSchoolService(IUnitOfWork unitOfWork,IHighSchoolRepository repository):base(unitOfWork,repository){}
    }
}
