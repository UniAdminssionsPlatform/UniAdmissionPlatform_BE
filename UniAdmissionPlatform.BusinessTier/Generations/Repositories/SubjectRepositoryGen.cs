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
    public partial interface ISubjectRepository :IBaseRepository<Subject>
    {
    }
    public partial class SubjectRepository :BaseRepository<Subject>, ISubjectRepository
    {
         public SubjectRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

