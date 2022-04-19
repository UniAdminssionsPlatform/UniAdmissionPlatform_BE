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
    
    public partial interface IOrganizationService:IBaseService<Organization>
    {
    }
    public partial class OrganizationService:BaseService<Organization>,IOrganizationService
    {
        public OrganizationService(IUnitOfWork unitOfWork,IOrganizationRepository repository):base(unitOfWork,repository){}
    }
}
