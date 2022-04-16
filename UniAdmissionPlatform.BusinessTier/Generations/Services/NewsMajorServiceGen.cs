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
    
    public partial interface INewsMajorService:IBaseService<NewsMajor>
    {
    }
    public partial class NewsMajorService:BaseService<NewsMajor>,INewsMajorService
    {
        public NewsMajorService(IUnitOfWork unitOfWork,INewsMajorRepository repository):base(unitOfWork,repository){}
    }
}
