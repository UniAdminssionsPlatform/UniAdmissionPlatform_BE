using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Certification;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class CertificationModule
    {
        public static void ConfigCertificationMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Certification, CertificationBaseViewModel>();
            mc.CreateMap<CreateCertificationRequest, Certification>();
            mc.CreateMap<UpdateCertificationRequest, Certification>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}