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
    public partial interface IHighSchoolEventRepository :IBaseRepository<HighSchoolEvent>
    {
    }
    public partial class HighSchoolEventRepository :BaseRepository<HighSchoolEvent>, IHighSchoolEventRepository
    {
         public HighSchoolEventRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

