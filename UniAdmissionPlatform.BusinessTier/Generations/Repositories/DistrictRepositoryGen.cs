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
    public partial interface IDistrictRepository :IBaseRepository<District>
    {
    }
    public partial class DistrictRepository :BaseRepository<District>, IDistrictRepository
    {
         public DistrictRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

