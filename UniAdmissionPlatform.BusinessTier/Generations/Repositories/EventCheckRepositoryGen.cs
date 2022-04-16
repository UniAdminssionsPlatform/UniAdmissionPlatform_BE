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
    public partial interface IEventCheckRepository :IBaseRepository<EventCheck>
    {
    }
    public partial class EventCheckRepository :BaseRepository<EventCheck>, IEventCheckRepository
    {
         public EventCheckRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

