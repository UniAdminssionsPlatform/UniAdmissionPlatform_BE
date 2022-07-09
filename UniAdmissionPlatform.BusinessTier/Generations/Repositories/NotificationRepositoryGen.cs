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
    public partial interface INotificationRepository :IBaseRepository<Notification>
    {
    }
    public partial class NotificationRepository :BaseRepository<Notification>, INotificationRepository
    {
         public NotificationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

