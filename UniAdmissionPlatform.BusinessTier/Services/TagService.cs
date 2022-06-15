using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Requests.Tag;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ITagService
    {
        Task<int> CreateTag(CreateTagRequest createTagRequest);
        Task UpdateTag(int id, UpdateTagRequest updateTagRequest);
        Task DeleteTag(int id);
        Task<PageResult<TagBaseViewModel>> GetAllTags(TagBaseViewModel filter, string sort,
            int page, int limit);
        Task<TagBaseViewModel> GetTagById(int tagId);
    }
    
    public partial class TagService
    {
        private readonly IConfigurationProvider _mapper;
        
        public TagService(IUnitOfWork unitOfWork, ITagRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateTag(CreateTagRequest createTagRequest)
        {
            var mapper = _mapper.CreateMapper();
            var tag = mapper.Map<Tag>(createTagRequest);
            
            tag.CreatedAt = DateTime.Now;
            tag.UpdatedAt = DateTime.Now;
            
            await CreateAsyn(tag);
            return tag.Id;
        }

        public async Task UpdateTag(int id, UpdateTagRequest updateTagRequest)
        {
            var tag = await Get().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tag với id = {id}");
            }

            var mapper = _mapper.CreateMapper();
            var tagInRequest = mapper.Map<Tag>(updateTagRequest);

            tag.Name = tagInRequest.Name;
            tag.UpdatedAt = DateTime.Now;
        
            await UpdateAsyn(tag);
        }

        public async Task DeleteTag(int id)
        {
            var tag = await Get().Where(t => t.Id == id && t.DeletedAt == null).FirstOrDefaultAsync();
            if (tag == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tag với id = {id}");
            }
            
            tag.DeletedAt = DateTime.Now;
            
            await UpdateAsyn(tag);
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<TagBaseViewModel>> GetAllTags(TagBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(t => t.DeletedAt == null).ProjectTo<TagBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<TagBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<TagBaseViewModel> GetTagById(int tagId)
        {
            var tagById = await Get().Where(t => t.Id == tagId && t.DeletedAt == null)
                .ProjectTo<TagBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (tagById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy tag nào nào có id = {tagId}");
            }
            return tagById;
        }
    }
}