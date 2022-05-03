using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface INationalityService
    {
        Task<PageResult<NationalityBaseViewModel>> GetAllNationalities(NationalityBaseViewModel filter, string sort,
            int page, int limit);
        
    }
    
    public partial class NationalityService
    {
        private readonly IConfigurationProvider _mapper;
        
        public NationalityService(IUnitOfWork unitOfWork, INationalityRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 300;

        public async Task<PageResult<NationalityBaseViewModel>> GetAllNationalities(NationalityBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<NationalityBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<NationalityBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        
    }
}