using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IProvinceService
    {
        Task<PageResult<ProvinceBaseViewModel>> GetAllProvinces(ProvinceBaseViewModel filter, string sort,
            int page, int limit);

        Task<ProvinceBaseViewModel> GetById(int id);
    }
    
    public partial class ProvinceService
    {
        private readonly IConfigurationProvider _mapper;
        
        public ProvinceService(IUnitOfWork unitOfWork, IProvinceRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<ProvinceBaseViewModel>> GetAllProvinces(ProvinceBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().ProjectTo<ProvinceBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<ProvinceBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<ProvinceBaseViewModel> GetById(int id)
        {
            var province = await FirstOrDefaultAsyn(p => p.Id == id);
            if (province == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy tỉnh thành");
            }

            return _mapper.CreateMapper().Map<ProvinceBaseViewModel>(province);
        }
    }
}