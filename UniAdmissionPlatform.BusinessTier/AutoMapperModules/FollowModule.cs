using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Follow;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class FollowModule
    {
        public static void ConfigFollowMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Follow, FollowBaseViewModel>();
            mc.CreateMap<CreateFollowRequest, Follow>();
        }
    }
}