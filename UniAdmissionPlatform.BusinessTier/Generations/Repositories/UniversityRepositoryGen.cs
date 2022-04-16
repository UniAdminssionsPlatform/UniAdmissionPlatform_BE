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
    public partial interface IUniversityRepository :IBaseRepository<University>
    {
    }
    public partial class UniversityRepository :BaseRepository<University>, IUniversityRepository
    {
         public UniversityRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

