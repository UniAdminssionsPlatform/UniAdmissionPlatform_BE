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
    public partial interface ISubjectGroupSubjectRepository :IBaseRepository<SubjectGroupSubject>
    {
    }
    public partial class SubjectGroupSubjectRepository :BaseRepository<SubjectGroupSubject>, ISubjectGroupSubjectRepository
    {
         public SubjectGroupSubjectRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

