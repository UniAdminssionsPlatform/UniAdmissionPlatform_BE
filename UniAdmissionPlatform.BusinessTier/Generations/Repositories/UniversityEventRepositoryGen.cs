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
    public partial interface IUniversityEventRepository :IBaseRepository<UniversityEvent>
    {
    }
    public partial class UniversityEventRepository :BaseRepository<UniversityEvent>, IUniversityEventRepository
    {
         public UniversityEventRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

