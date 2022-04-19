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
    
    public partial interface IUniversityNewsService:IBaseService<UniversityNews>
    {
    }
    public partial class UniversityNewsService:BaseService<UniversityNews>,IUniversityNewsService
    {
        public UniversityNewsService(IUnitOfWork unitOfWork,IUniversityNewsRepository repository):base(unitOfWork,repository){}
    }
}
