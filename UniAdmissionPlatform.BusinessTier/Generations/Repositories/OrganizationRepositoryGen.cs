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
    public partial interface IOrganizationRepository :IBaseRepository<Organization>
    {
    }
    public partial class OrganizationRepository :BaseRepository<Organization>, IOrganizationRepository
    {
         public OrganizationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

