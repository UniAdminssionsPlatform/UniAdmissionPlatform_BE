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
    
    public partial interface IUniversityService:IBaseService<University>
    {
    }
    public partial class UniversityService:BaseService<University>,IUniversityService
    {
        public UniversityService(IUnitOfWork unitOfWork,IUniversityRepository repository, IFollowRepository followRepository):base(unitOfWork,repository)
        {
            _followRepository = followRepository;
        }
    }
}
