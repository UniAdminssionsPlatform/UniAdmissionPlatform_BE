using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class ProvinceModule
    {
        public static void ConfigProvinceMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Province, ProvinceBaseViewModel>();
        }
    }
}