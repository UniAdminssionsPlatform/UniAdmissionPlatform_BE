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
    
    public partial interface INewsService:IBaseService<News>
    {
    }
    public partial class NewsService:BaseService<News>,INewsService
    {
        public NewsService(IUnitOfWork unitOfWork,INewsRepository repository):base(unitOfWork,repository){}
    }
}
