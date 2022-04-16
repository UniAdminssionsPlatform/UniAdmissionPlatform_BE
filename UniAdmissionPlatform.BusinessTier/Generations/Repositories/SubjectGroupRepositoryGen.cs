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
    public partial interface ISubjectGroupRepository :IBaseRepository<SubjectGroup>
    {
    }
    public partial class SubjectGroupRepository :BaseRepository<SubjectGroup>, ISubjectGroupRepository
    {
         public SubjectGroupRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

