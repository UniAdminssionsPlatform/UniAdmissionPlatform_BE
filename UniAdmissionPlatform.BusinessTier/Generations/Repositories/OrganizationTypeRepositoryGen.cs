/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
namespace UniAdmissionPlatform.BusinessTier.Generations.Repositories
{
    public partial interface IOrganizationTypeRepository :IBaseRepository<OrganizationType>
    {
    }
    public partial class OrganizationTypeRepository :BaseRepository<OrganizationType>, IOrganizationTypeRepository
    {
         public OrganizationTypeRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

