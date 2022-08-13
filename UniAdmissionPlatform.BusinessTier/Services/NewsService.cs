using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.News;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface INewsService
    {
        Task<PageResult<NewsWithUniversityViewModel>> GetAllNews(NewsWithUniversityViewModel filter, string sort, int page, int limit);
        Task<NewsWithUniversityViewModel> GetNewsById(int newsId);
        Task<int> CreateNews(int universityId, CreateNewsRequest createNewsRequest);
        Task UpdateNews(int newsId, UpdateNewsRequest updateNewsRequest);
        Task DeleteNewsById(int newsId);

        Task<PageResult<NewsWithPublishViewModel>> GetAllNewsForUniversityAdmin(NewsWithPublishViewModel filter,
            string sort, int page, int limit, int universityId);

        Task SetIsPublish(int universityId, int newsId, bool isPublish);
    }
    public partial class NewsService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly ITagRepository _tagRepository;

        public NewsService(IUnitOfWork unitOfWork, INewsRepository repository, IMapper mapper, ITagRepository tagRepository) : base(unitOfWork,
            repository)
        {
            _tagRepository = tagRepository;
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<NewsWithUniversityViewModel>> GetAllNews(NewsWithUniversityViewModel filter, string sort, int page, int limit)
        {
            List<int> tagIds = null;
            if (filter.Tags != null)
            {
                var substring = filter.Tags.Substring(1, filter.Tags.Length - 2);
                tagIds = substring.Split(",").Select(int.Parse).ToList();
            }
            var (total, queryable) = Get()
                .Where(n => n.DeletedAt == null && n.IsPublish != null && n.IsPublish.Value
                            && (tagIds == null || n.NewsTags.Select(nt => nt.TagId).Any(ti => tagIds.Contains(ti)))
                )
                .ProjectTo<NewsWithUniversityViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<NewsWithUniversityViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<NewsWithPublishViewModel>> GetAllNewsForUniversityAdmin(NewsWithPublishViewModel filter, string sort, int page, int limit, int universityId)
        {
            List<int> tagIds = null;
            if (filter.Tags != null)
            {
                var substring = filter.Tags.Substring(1, filter.Tags.Length - 2);
                tagIds = substring.Split(",").Select(int.Parse).ToList();
            }
            var (total, queryable) = Get()
                .Where(n => n.DeletedAt == null && n.UniversityId == universityId
                                                && (tagIds == null || n.NewsTags.Select(nt => nt.TagId).Any(ti => tagIds.Contains(ti)))
                )
                .ProjectTo<NewsWithPublishViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<NewsWithPublishViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task SetIsPublish(int universityId, int newsId, bool isPublish)
        {
            var news = await Get()
                .FirstOrDefaultAsync(n => n.DeletedAt == null && n.Id == newsId && n.UniversityId == universityId);

            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, $"Không tìm thấy tin tức với id = {newsId}.");
            }

            news.IsPublish = isPublish;
            news.UpdatedAt = DateTime.Now;

            await UpdateAsyn(news);
        }

        public async Task<NewsWithUniversityViewModel> GetNewsById(int newsId)
        {
            var news = await Get()
                .Where(n => n.Id == newsId && n.DeletedAt == null)
                .ProjectTo<NewsWithUniversityViewModel>(_mapper)
                .FirstOrDefaultAsync();
            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tin tức id = {newsId}.");
            }
            
            return news;
        }

        public async Task<int> CreateNews(int universityId, CreateNewsRequest createNewsRequest)
        {
            var news = _mapper.CreateMapper().Map<News>(createNewsRequest);

            var tags = await _tagRepository.Get()
                .Where(t => createNewsRequest.TagIds.Contains(t.Id))
                .ToListAsync();
            if (tags.Count != createNewsRequest.TagIds.Count)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Một số tag không khả dụng.");
            }

            news.CreateDate = DateTime.Now;
            news.CreatedAt = DateTime.Now;
            news.UpdatedAt = DateTime.Now;
            news.UniversityId = universityId;
            await CreateAsyn(news);
            
            return news.Id;
        }

        public async Task UpdateNews(int newsId, UpdateNewsRequest updateNewsRequest)
        {
            var news = await Get()
                .Where(n => n.Id == newsId && n.DeletedAt == null)
                .FirstOrDefaultAsync();
            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tin tức với id = {newsId}.");
            }

            var mapper = _mapper.CreateMapper();
            news = mapper.Map(updateNewsRequest,news);
            news.UpdatedAt =DateTime.Now;

            await UpdateAsyn(news);
        }

        public async Task DeleteNewsById(int newsId)
        {
            var news = await Get()
                .Where(n => n.Id == newsId && n.DeletedAt == null)
                .Include(n => n.NewsMajors)
                .Include(n => n.NewsTags).FirstOrDefaultAsync();
            
            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tin tức với id = {newsId}.");
            }
            
            news.DeletedAt = DateTime.Now;
            news.NewsTags = new List<NewsTag>();
            news.NewsMajors = new List<NewsMajor>();
            await UpdateAsyn(news);
        }
    }
}