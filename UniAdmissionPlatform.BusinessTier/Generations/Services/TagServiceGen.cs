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
    
    public partial interface ITagService:IBaseService<Tag>
    {
    }
    public partial class TagService:BaseService<Tag>,ITagService
    {
        public TagService(IUnitOfWork unitOfWork,ITagRepository repository):base(unitOfWork,repository){}
    }
}
