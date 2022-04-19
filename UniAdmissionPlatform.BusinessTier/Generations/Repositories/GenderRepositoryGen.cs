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
    public partial interface IGenderRepository :IBaseRepository<Gender>
    {
    }
    public partial class GenderRepository :BaseRepository<Gender>, IGenderRepository
    {
         public GenderRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

