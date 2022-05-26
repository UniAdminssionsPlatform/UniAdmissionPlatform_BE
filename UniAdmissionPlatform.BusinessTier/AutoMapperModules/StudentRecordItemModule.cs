using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class StudentRecordItemModule
    {
        public static void ConfigStudentRecordItemMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<StudentRecordItem, StudentRecordItemWithSubjectModel>()
                .ForMember(des => des.Subject, opt
                => opt.MapFrom(src => src.Subject));
        }
    }
}