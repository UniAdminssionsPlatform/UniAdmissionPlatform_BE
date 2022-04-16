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
    
    public partial interface ISchoolYearService:IBaseService<SchoolYear>
    {
    }
    public partial class SchoolYearService:BaseService<SchoolYear>,ISchoolYearService
    {
        public SchoolYearService(IUnitOfWork unitOfWork,ISchoolYearRepository repository):base(unitOfWork,repository){}
    }
}
