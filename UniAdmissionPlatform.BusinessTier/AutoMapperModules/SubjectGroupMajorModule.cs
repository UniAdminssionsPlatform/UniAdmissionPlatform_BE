using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroupMajor;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SubjectGroupMajorModule
    {
        public static void ConfigSubjectGroupMajorMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<SubjectGroupMajor, SubjectGroupMajorBaseViewModel>();
            mc.CreateMap<CreateSubjectGroupMajorRequest, SubjectGroupMajor>();
        }
    }
}