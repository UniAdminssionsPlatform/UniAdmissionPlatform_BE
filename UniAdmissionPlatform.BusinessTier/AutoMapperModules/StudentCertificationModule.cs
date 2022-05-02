using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.StudentCertification;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class StudentCertificationModule
    {
        public static void ConfigStudentCertificationMapperModule(this IMapperConfigurationExpression mc)
            {
                mc.CreateMap<StudentCertification, StudentCertificationBaseViewModel>();
                mc.CreateMap<CreateStudentCertificationRequest, StudentCertification>();
                mc.CreateMap<UpdateStudentCertificationRequest, StudentCertification>();
            }
    }
}