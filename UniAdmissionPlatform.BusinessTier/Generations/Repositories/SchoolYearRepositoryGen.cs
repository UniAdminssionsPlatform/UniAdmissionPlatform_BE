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
    public partial interface ISchoolYearRepository :IBaseRepository<SchoolYear>
    {
    }
    public partial class SchoolYearRepository :BaseRepository<SchoolYear>, ISchoolYearRepository
    {
         public SchoolYearRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

