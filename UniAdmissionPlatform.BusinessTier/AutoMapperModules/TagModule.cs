using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Tag;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class TagModule
    {
        public static void ConfigTagMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateTagRequest, Tag>();
            mc.CreateMap<Tag, TagBaseViewModel>();
            mc.CreateMap<UpdateTagRequest, Tag>();
        }
    }
}