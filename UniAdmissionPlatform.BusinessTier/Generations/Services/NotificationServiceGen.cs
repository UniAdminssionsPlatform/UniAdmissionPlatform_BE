/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    
    public partial interface INotificationService:IBaseService<Notification>
    {
    }
    public partial class NotificationService:BaseService<Notification>,INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork,INotificationRepository repository):base(unitOfWork,repository){}
    }
}
