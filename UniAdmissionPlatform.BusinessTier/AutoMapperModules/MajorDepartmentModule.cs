using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.MajorDepartment;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class MajorDepartmentModule
    {
        public static void ConfigMajorDepartmentModuleMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<MajorDepartment, MajorDepartmentBaseViewModel>();
            mc.CreateMap<CreateMajorDepartmentRequest, MajorDepartment>();
            mc.CreateMap<UpdateMajorDepartmentRequest, MajorDepartment>()
                .ForAllMembers(opt => opt
                    .Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}