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
    public partial interface IEventRepository :IBaseRepository<Event>
    {
    }
    public partial class EventRepository :BaseRepository<Event>, IEventRepository
    {
         public EventRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

