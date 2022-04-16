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
    public partial interface IUniversityNewsRepository :IBaseRepository<UniversityNews>
    {
    }
    public partial class UniversityNewsRepository :BaseRepository<UniversityNews>, IUniversityNewsRepository
    {
         public UniversityNewsRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

