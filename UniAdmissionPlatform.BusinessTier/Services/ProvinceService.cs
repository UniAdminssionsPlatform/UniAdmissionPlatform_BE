using System.Linq;
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

        Task<ProvinceBaseViewModel> GetProvinceByID(int provinceId);
    }
    
    public partial class ProvinceService
    {
        private readonly IConfigurationProvider _mapper;
        
        public ProvinceService(IUnitOfWork unitOfWork, IProvinceRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 100;
        private const int DefaultPaging = 63;

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
        
        public async Task<ProvinceBaseViewModel> GetProvinceByID(int provinceId)
        {
            var provinceById = await Get().Where(p => p.Id == provinceId)
                .ProjectTo<ProvinceBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (provinceById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy tỉnh thành nào nào có id = {provinceId}");
            }
            return provinceById;
        }
    }
}