using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class UniversityEventModule
    {
        public static void ConfigUniversityEventMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<UniversityEvent, EventByUniIdBaseViewModel>()
                .ForMember(des => des.Event,
                    opt => opt.MapFrom(src => src.Event))
                .ForMember(des => des.UniversityId, 
                    opt => opt.MapFrom(src => src.UniversityId));
        }
    }
}