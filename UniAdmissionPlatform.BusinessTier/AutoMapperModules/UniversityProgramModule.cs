using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class UniversityProgramModule
    {
        public static void ConfigUniversityProgramModuleMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<UniversityProgram, UniversityProgramBaseViewModel>();
            mc.CreateMap<CreateUniversityProgramRequest, UniversityProgram>();
            mc.CreateMap<UpdateUniversityProgramRequest, UniversityProgram>()
                .ForAllMembers(opt => opt
                    .Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<UniversityProgram, UniversityProgramWithMajorDepartmentAndSchoolYearModel>()
                .ForMember(des => des.MajorDepartment,
                    opt =>
                        opt.MapFrom(src => src.MajorDepartment))
                .ForMember(des => des.SchoolYear,
                    opt =>
                        opt.MapFrom(src => src.SchoolYear));
            mc.CreateMap<UniversityProgram, UniversityProgramAdmission>()
                .ForMember(des => des.ParentId, opt =>
                    opt.MapFrom(src =>
                        src.MajorDepartment.MajorParentId))
                .ForMember(des => des.ParentCode, opt =>
                    opt.MapFrom(src =>
                        src.MajorDepartment.MajorParent.Code))
                .ForMember(des => des.ParentName, opt =>
                    opt.MapFrom(src =>
                        src.MajorDepartment.MajorParent.Name))
                .ForMember(des => des.MajorId, opt => opt.MapFrom(
                    src => src.MajorDepartment.Major.Id))
                .ForMember(des => des.MajorCode, opt => opt.MapFrom(
                    src => src.MajorDepartment.Major.Code))
                .ForMember(des => des.MajorName, opt => opt.MapFrom(
                    src => src.MajorDepartment.Major.Name))
                .ForMember(des => des.SubjectGroupId, opt => opt.MapFrom(
                    src => src.SubjectGroup.Id))
                .ForMember(des => des.SubjectGroupName, opt => opt.MapFrom(
                    src => src.SubjectGroup.Name));



        }
    }
}