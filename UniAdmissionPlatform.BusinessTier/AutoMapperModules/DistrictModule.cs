using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class DistrictModule
    {
        public static void ConfigDistrictMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<District, DistrictViewModel>();
        }
    }
}