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
    public partial interface IEventTypeRepository :IBaseRepository<EventType>
    {
    }
    public partial class EventTypeRepository :BaseRepository<EventType>, IEventTypeRepository
    {
         public EventTypeRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

