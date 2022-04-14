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
    public partial interface ISubjectGroupMajorRepository :IBaseRepository<SubjectGroupMajor>
    {
    }
    public partial class SubjectGroupMajorRepository :BaseRepository<SubjectGroupMajor>, ISubjectGroupMajorRepository
    {
         public SubjectGroupMajorRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

