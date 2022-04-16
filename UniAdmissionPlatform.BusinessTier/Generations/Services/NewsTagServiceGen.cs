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
    
    public partial interface INewsTagService:IBaseService<NewsTag>
    {
    }
    public partial class NewsTagService:BaseService<NewsTag>,INewsTagService
    {
        public NewsTagService(IUnitOfWork unitOfWork,INewsTagRepository repository):base(unitOfWork,repository){}
    }
}
