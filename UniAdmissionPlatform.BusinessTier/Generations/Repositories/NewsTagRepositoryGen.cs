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
    public partial interface INewsTagRepository :IBaseRepository<NewsTag>
    {
    }
    public partial class NewsTagRepository :BaseRepository<NewsTag>, INewsTagRepository
    {
         public NewsTagRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

