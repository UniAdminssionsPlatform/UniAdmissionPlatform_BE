using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Requests.University;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class UniversityModule
    {
        public static void ConfigUniversityMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<University, UniversityBaseViewModel>();
            mc.CreateMap<University, UniversityCodeViewModel>();
            mc.CreateMap<CreateUniversityRequest, University>();
        }
    }
}