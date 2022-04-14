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
    
    public partial interface IParticipationService:IBaseService<Participation>
    {
    }
    public partial class ParticipationService:BaseService<Participation>,IParticipationService
    {
        public ParticipationService(IUnitOfWork unitOfWork,IParticipationRepository repository):base(unitOfWork,repository){}
    }
}
