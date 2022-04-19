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
    public partial interface IUniversityProgramRepository :IBaseRepository<UniversityProgram>
    {
    }
    public partial class UniversityProgramRepository :BaseRepository<UniversityProgram>, IUniversityProgramRepository
    {
         public UniversityProgramRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

