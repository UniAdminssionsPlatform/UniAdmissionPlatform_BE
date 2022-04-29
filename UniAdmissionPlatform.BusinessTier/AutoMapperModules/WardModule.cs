using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class WardModule
    {
        public static void ConfigWardsMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Ward, WardViewModel>();
        }
    }
}