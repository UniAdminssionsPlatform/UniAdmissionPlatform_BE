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
    public partial interface IWardRepository :IBaseRepository<Ward>
    {
    }
    public partial class WardRepository :BaseRepository<Ward>, IWardRepository
    {
         public WardRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

