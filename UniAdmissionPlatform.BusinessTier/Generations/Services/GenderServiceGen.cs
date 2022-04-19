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
    
    public partial interface IGenderService:IBaseService<Gender>
    {
    }
    public partial class GenderService:BaseService<Gender>,IGenderService
    {
        public GenderService(IUnitOfWork unitOfWork,IGenderRepository repository):base(unitOfWork,repository){}
    }
}
