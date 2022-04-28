using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Subject;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SubjectModule
    {
        public static void ConfigSubjectMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Subject, SubjectBaseViewModel>();
            mc.CreateMap<CreateSubjectRequest, Subject>();
            mc.CreateMap<UpdateSubjectRequest, Subject>();
        }
    }
}