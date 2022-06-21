using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IFollowService:IBaseService<Follow>
    {
        
    }
    public partial class FollowService:BaseService<Follow>,IFollowService
    {
        public FollowService(IUnitOfWork unitOfWork,IFollowRepository repository):base(unitOfWork,repository){}
    }
}