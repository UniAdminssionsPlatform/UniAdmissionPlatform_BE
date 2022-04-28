﻿using System;
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
        Task<PageResult<NewsBaseViewModel>> GetAllNews(NewsBaseViewModel filter, string sort, int page, int limit);
        Task<NewsBaseViewModel> GetNewsById(int newsId);
        Task<int> CreateNews(int universityId, CreateNewsRequest createNewsRequest);
        Task UpdateNews(int newsId, UpdateNewsRequest updateNewsRequest);
        Task DeleteNewsById(int newsId);
    }
    public partial class NewsService
    {
        private readonly IConfigurationProvider _mapper;

        public NewsService(IUnitOfWork unitOfWork, INewsRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<NewsBaseViewModel>> GetAllNews(NewsBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<NewsBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<NewsBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<NewsBaseViewModel> GetNewsById(int newsId)
        {
            var news = await Get().Where(n => n.Id == newsId).FirstOrDefaultAsync();
            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tin tức id = {newsId}.");
            }
            
            return _mapper.CreateMapper().Map<NewsBaseViewModel>(news);
        }

        public async Task<int> CreateNews(int universityId, CreateNewsRequest createNewsRequest)
        {
            var news = _mapper.CreateMapper().Map<News>(createNewsRequest);

            var uniNewsUniversityNews = news.UniversityNews = new List<UniversityNews>();
            uniNewsUniversityNews.Add(new UniversityNews
            {
                UniversityId = universityId,
            });
            
            news.CreateDate = DateTime.Now;
            news.CreatedAt = DateTime.Now;
            news.UpdatedAt = DateTime.Now;
            await CreateAsyn(news);
            
            return news.Id;
        }

        public async Task UpdateNews(int newsId, UpdateNewsRequest updateNewsRequest)
        {
            var news = await Get().Where(n => n.Id == newsId && n.DeletedAt == null).FirstOrDefaultAsync();
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
            var news = await Get().Where(n => n.Id == newsId && n.DeletedAt == null).FirstOrDefaultAsync();
            if (news == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tin tức với id = {newsId}.");
            }
            news.DeletedAt =DateTime.Now;
            await UpdateAsyn(news);
        }
    }
}