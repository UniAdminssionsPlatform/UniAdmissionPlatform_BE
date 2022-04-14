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
    public partial interface IProvinceRepository :IBaseRepository<Province>
    {
    }
    public partial class ProvinceRepository :BaseRepository<Province>, IProvinceRepository
    {
         public ProvinceRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

