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
        }
    }
}