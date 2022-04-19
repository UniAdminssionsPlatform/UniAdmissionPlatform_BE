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
    public partial interface INewsRepository :IBaseRepository<News>
    {
    }
    public partial class NewsRepository :BaseRepository<News>, INewsRepository
    {
         public NewsRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

