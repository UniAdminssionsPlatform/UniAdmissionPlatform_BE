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
    
    public partial interface IOrganizationTypeService:IBaseService<OrganizationType>
    {
    }
    public partial class OrganizationTypeService:BaseService<OrganizationType>,IOrganizationTypeService
    {
        public OrganizationTypeService(IUnitOfWork unitOfWork,IOrganizationTypeRepository repository):base(unitOfWork,repository){}
    }
}
