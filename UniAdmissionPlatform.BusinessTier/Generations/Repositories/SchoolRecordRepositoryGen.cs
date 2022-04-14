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
    public partial interface ISchoolRecordRepository :IBaseRepository<SchoolRecord>
    {
    }
    public partial class SchoolRecordRepository :BaseRepository<SchoolRecord>, ISchoolRecordRepository
    {
         public SchoolRecordRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

