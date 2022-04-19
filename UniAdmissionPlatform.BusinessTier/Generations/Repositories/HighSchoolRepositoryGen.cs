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
    public partial interface IHighSchoolRepository :IBaseRepository<HighSchool>
    {
    }
    public partial class HighSchoolRepository :BaseRepository<HighSchool>, IHighSchoolRepository
    {
         public HighSchoolRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

