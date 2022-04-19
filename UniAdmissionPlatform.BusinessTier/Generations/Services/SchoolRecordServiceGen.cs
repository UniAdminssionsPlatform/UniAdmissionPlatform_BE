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
    
    public partial interface ISchoolRecordService:IBaseService<SchoolRecord>
    {
    }
    public partial class SchoolRecordService:BaseService<SchoolRecord>,ISchoolRecordService
    {
        public SchoolRecordService(IUnitOfWork unitOfWork,ISchoolRecordRepository repository):base(unitOfWork,repository){}
    }
}
