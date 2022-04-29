using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IWardService
    {
        Task<PageResult<WardViewModel>> GetAllWards(WardViewModel filter, string sort, int page,
            int limit);
    }

    public partial class WardService
    {
        private readonly IConfigurationProvider _mapper;

        public WardService(IUnitOfWork unitOfWork, IWardRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 200;
        private const int DefaultPaging = 100;

        public async Task<PageResult<WardViewModel>> GetAllWards(WardViewModel filter, string sort, int page,
            int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<WardViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<WardViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}