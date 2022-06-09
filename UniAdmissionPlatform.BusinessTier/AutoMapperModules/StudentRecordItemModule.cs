using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class StudentRecordItemModule
    {
        public static void ConfigStudentRecordItemMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<StudentRecordItem, StudentRecordItemBaseViewModel>();
            mc.CreateMap<CreateStudentRecordItemRequest, StudentRecordItem>();
            mc.CreateMap<UpdateStudentRecordItemRequest, StudentRecordItem>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<StudentRecordItem, StudentRecordItemWithSubjectModel>()
                .ForMember(des => des.Subject, opt
                => opt.MapFrom(src => src.Subject));
            mc.CreateMap<CreateStudentRecordItemBase, StudentRecordItem>();
            mc.CreateMap<UpdateStudentRecordItemBase, StudentRecordItem>();
        }
    }
}