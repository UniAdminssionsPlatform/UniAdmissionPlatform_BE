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
    public partial interface IRegionRepository :IBaseRepository<Region>
    {
    }
    public partial class RegionRepository :BaseRepository<Region>, IRegionRepository
    {
         public RegionRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

