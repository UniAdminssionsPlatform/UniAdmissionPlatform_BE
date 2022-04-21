using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class UniversitytModule
    {
        public static void ConfigUniversityMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<University, UniversityBaseViewModel>();
        }
    }
}