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
            mc.CreateMap<CreateSchoolRecordRequest, SchoolRecord>();
            mc.CreateMap<UpdateSchoolRecordRequest, SchoolRecord>()
                .ForAllMembers(opt => opt
                .Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}