using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.News;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class NewsModule
    {
        public static void ConfigNewsMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<News, NewsBaseViewModel>();
            mc.CreateMap<UpdateNewsRequest, News>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<CreateNewsRequest, News>();
            mc.CreateMap<News, NewsWithPublishViewModel>();
        }
    }
}