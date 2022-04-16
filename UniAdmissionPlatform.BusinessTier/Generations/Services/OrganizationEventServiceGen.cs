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
    
    public partial interface IOrganizationEventService:IBaseService<OrganizationEvent>
    {
    }
    public partial class OrganizationEventService:BaseService<OrganizationEvent>,IOrganizationEventService
    {
        public OrganizationEventService(IUnitOfWork unitOfWork,IOrganizationEventRepository repository):base(unitOfWork,repository){}
    }
}
