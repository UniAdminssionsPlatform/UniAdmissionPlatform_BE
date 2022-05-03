using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class NationalityModule
    {
        public static void ConfigNationalityMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Nationality, NationalityBaseViewModel>();
        }
    }
}