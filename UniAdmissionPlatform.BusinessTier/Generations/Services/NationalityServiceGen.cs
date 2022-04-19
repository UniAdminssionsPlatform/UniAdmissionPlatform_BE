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
    
    public partial interface INationalityService:IBaseService<Nationality>
    {
    }
    public partial class NationalityService:BaseService<Nationality>,INationalityService
    {
        public NationalityService(IUnitOfWork unitOfWork,INationalityRepository repository):base(unitOfWork,repository){}
    }
}
