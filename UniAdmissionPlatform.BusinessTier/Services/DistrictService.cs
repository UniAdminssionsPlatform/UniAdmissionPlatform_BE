using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IDistrictService
    {
        Task<PageResult<DistrictViewModel>> GetAllDistrict(DistrictViewModel filter, string sort, int page, int limit);
        Task<DistrictViewModel> GetDistrictById(int id);
    }
    public partial class DistrictService
    {
        private readonly IConfigurationProvider _mapper;

        public DistrictService(IUnitOfWork unitOfWork, IDistrictRepository repository, IMapper mapper) : base(
            unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 200;
        private const int DefaultPaging = 100;

        public async Task<PageResult<DistrictViewModel>> GetAllDistrict(DistrictViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<DistrictViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<DistrictViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<DistrictViewModel> GetDistrictById(int id)
        {
            var district = await FirstOrDefaultAsyn(d => d.Id == id);
            if (district == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không thể tìm thấy quận huyện có id = {id}");
            }

            return _mapper.CreateMapper().Map<DistrictViewModel>(district);
        }
    }
}