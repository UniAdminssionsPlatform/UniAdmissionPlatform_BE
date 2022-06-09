using System.Linq;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord;
using UniAdmissionPlatform.BusinessTier.Requests.StudentCertification;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SchoolRecordModule
    {
        public static void ConfigSchoolRecordMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<SchoolRecord, SchoolRecordBaseViewModel>();
            mc.CreateMap<CreateSchoolRecordRequest, SchoolRecord>()
                .ForMember(des => des.StudentRecordItems,
                    opt => opt.MapFrom(
                        src => src.RecordItems.Select(ri => new StudentRecordItem
                        {
                            Score = ri.Score,
                            SubjectId = ri.SubjectId ?? 0
                        })));
            mc.CreateMap<UpdateSchoolRecordRequest, SchoolRecord>()
                .ForAllMembers(opt => opt
                .Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<SchoolRecord, SchoolRecordWithStudentRecordItemModel>()
            .ForMember(des => des.StudentRecordItems, opt
                => opt.MapFrom(src => src.StudentRecordItems));

            mc.CreateMap<UpdateSchoolRecordRequest, SchoolRecord>();
        }
    }
}