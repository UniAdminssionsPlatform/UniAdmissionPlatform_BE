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
    
    public partial interface IHighSchoolEventService:IBaseService<HighSchoolEvent>
    {
    }
    public partial class HighSchoolEventService:BaseService<HighSchoolEvent>,IHighSchoolEventService
    {
        public HighSchoolEventService(IUnitOfWork unitOfWork,IHighSchoolEventRepository repository):base(unitOfWork,repository){}
    }
}
