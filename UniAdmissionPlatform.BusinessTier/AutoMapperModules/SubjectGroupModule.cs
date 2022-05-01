using System.Linq;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.MajorGroup;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroup;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SubjectGroupModule
    {
        public static void ConfigSubjectGroupMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<SubjectGroup, SubjectGroupBaseViewModel>();
            mc.CreateMap<SubjectGroup, SubjectGroupWithSubject>()
                .ForMember(des => des.Subjects, opt => opt.MapFrom(
                    src => src.SubjectGroupSubjects.Select(sgs => sgs.Subject)));
            mc.CreateMap<CreateSubjectGroupRequest, SubjectGroup>();
            mc.CreateMap<UpdateSubjectGroupRequest, SubjectGroup>();
        }
    }
}