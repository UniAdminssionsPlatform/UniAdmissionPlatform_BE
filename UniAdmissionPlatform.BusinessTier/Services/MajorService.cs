using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Major;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IMajorService
    {
        Task<PageResult<MajorBaseViewModel>> GetAllMajor(MajorBaseViewModel filter, string sort, int page, int limit);
        Task<MajorBaseViewModel> GetMajorById(int id);
        Task<int> CreateMajor(CreateMajorRequest createMajorRequest);
        Task UpdateMajor(int id, UpdateMajorRequest updateMajorRequest);
        Task DeleteById(int id);
    }
    public partial class MajorService
    {
        private readonly IConfigurationProvider _mapper;

        public MajorService(IUnitOfWork unitOfWork, IMajorRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<MajorBaseViewModel>> GetAllMajor(MajorBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<MajorBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<MajorBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<MajorBaseViewModel> GetMajorById(int id)
        {
            var major = await FirstOrDefaultAsyn(m => m.Id == id);
            if (major == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy ngành học.");
            }

            return _mapper.CreateMapper().Map<MajorBaseViewModel>(major);
        }

        public async Task<int> CreateMajor(CreateMajorRequest createMajorRequest)
        {
            var major = _mapper.CreateMapper().Map<Major>(createMajorRequest);
            await CreateAsyn(major);
            return major.Id;
        }

        public async Task UpdateMajor(int id, UpdateMajorRequest updateMajorRequest)
        {
            var major = await FirstOrDefaultAsyn(m => m.Id == id);
            if (major == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy ngành học.");
            }

            major.Name = updateMajorRequest.Name;
            major.Code = updateMajorRequest.Code;
            major.MajorGroupId = updateMajorRequest.MajorGroupId;

            await UpdateAsyn(major);
        }

        public async Task DeleteById(int id)
        {
            var major = await FirstOrDefaultAsyn(m => m.Id == id);
            if (major == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy ngành học.");
            }

            await DeleteAsyn(major);
        }
    }
}