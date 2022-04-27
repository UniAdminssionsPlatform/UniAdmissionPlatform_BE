using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Major;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class MajorModule
    {
        public static void ConfigMajorMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Major, MajorBaseViewModel>();
            mc.CreateMap<UpdateMajorRequest, Major>();
            mc.CreateMap<CreateMajorRequest, Major>();
        }
    }
}