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
    public partial interface IMajorRepository :IBaseRepository<Major>
    {
    }
    public partial class MajorRepository :BaseRepository<Major>, IMajorRepository
    {
         public MajorRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

