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
    public partial interface IOrganizationEventRepository :IBaseRepository<OrganizationEvent>
    {
    }
    public partial class OrganizationEventRepository :BaseRepository<OrganizationEvent>, IOrganizationEventRepository
    {
         public OrganizationEventRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

