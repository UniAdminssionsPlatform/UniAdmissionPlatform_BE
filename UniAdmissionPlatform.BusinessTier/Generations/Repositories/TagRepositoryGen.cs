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
    public partial interface ITagRepository :IBaseRepository<Tag>
    {
    }
    public partial class TagRepository :BaseRepository<Tag>, ITagRepository
    {
         public TagRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

