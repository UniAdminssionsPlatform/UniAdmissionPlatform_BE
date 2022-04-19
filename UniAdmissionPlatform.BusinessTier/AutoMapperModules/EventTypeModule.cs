using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class EventTypeModule
    {
        public static void ConfigEventTypeMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<EventType, EventTypeBaseViewModel>();
        }
    }
}