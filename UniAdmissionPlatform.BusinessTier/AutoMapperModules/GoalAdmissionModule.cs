using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmission;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    
    public static class GoalAdmissionModule
    {
        public static void ConfigGoalAdmissionMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateGoalAdmissionRequest, GoalAdmission>();
            mc.CreateMap<GoalAdmission, GoalAdmissionBaseViewModel>();
            mc.CreateMap<UpdateGoalAdmissionRequest, GoalAdmission>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}