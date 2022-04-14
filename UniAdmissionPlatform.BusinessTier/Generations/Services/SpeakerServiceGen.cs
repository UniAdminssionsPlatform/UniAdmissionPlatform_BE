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
    
    public partial interface ISpeakerService:IBaseService<Speaker>
    {
    }
    public partial class SpeakerService:BaseService<Speaker>,ISpeakerService
    {
        public SpeakerService(IUnitOfWork unitOfWork,ISpeakerRepository repository):base(unitOfWork,repository){}
    }
}
