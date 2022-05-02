using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.News;
using UniAdmissionPlatform.BusinessTier.Requests.Participation;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class ParticipationModule
    {
        public static void ConfigParticipationMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Participation, ParticipationBaseViewModel>();
            mc.CreateMap<CreateParticipationRequestForStudent, Participation>();
            mc.CreateMap<UpdateParticipationRequestForStudent, Participation>();
        }
    }
}