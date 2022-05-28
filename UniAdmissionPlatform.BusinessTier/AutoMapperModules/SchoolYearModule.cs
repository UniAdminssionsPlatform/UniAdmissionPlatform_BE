using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SchoolYearModule
    {
        public static void ConfigSchoolYearMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<SchoolYear, SchoolYearBaseViewModel>();
        }
    }
}