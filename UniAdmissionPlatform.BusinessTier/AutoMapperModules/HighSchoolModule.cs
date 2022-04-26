using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class HighSchoolModule
    {
        public static void ConfigHighSchoolMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<HighSchool, HighSchoolBaseViewModel>();
            mc.CreateMap<HighSchool, HighSchoolCodeViewModel>();
            mc.CreateMap<HighSchool, GetHighSchoolBaseViewModel>();
        }
    }
}