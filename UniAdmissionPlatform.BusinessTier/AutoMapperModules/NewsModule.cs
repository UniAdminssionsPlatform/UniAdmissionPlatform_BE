using System.Linq;
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
            mc.CreateMap<News, NewsBaseViewModel>()
                .ForMember(des => des.TagList, opt =>
                    opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag)));
            mc.CreateMap<News, NewsWithUniversityViewModel>()
                .ForMember(des => des.TagList, opt =>
                    opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag)))
                .ForMember(des => des.University, opt =>
                    opt.MapFrom(src => src.UniversityNews.FirstOrDefault().University ?? new University()));
            mc.CreateMap<UpdateNewsRequest, News>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<CreateNewsRequest, News>()
                .ForMember(des => des.NewsTags, opt =>
                    opt.MapFrom(src => src.TagIds.Select(ti => new NewsTag
                    {
                        TagId = ti
                    })));
            mc.CreateMap<News, NewsWithPublishViewModel>()
                .ForMember(des => des.TagList, opt =>
                    opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag)));
        }
    }
}