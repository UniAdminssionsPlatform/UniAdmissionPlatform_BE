using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Notification;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class NotificationModule
    {
        public static void ConfigNotificationMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Notification, NotificationViewModel>();
            mc.CreateMap<CreateNotificationRequest, Notification>();
        }
    }
}