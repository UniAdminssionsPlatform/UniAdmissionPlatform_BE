using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmissionType;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    
        public static class GoalAdmissionTypeModule
        {
            public static void ConfigGoalAdmissionTypeMapperModule(this IMapperConfigurationExpression mc)
            {
                mc.CreateMap<CreateGoalAdmissionTypeRequest, GoalAdmissionType>();
                mc.CreateMap<GoalAdmissionType, GoalAdmissionTypeBaseViewModel>();
                mc.CreateMap<UpdateGoalAdmissionTypeRequest, GoalAdmissionType>()
                    .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            }
        }
}