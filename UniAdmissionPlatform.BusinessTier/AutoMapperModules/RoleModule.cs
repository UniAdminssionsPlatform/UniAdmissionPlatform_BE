using AutoMapper;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class RoleModule
    {
        public static void ConfigRoleMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Role, RoleBaseViewModel>();
        }
    }
}