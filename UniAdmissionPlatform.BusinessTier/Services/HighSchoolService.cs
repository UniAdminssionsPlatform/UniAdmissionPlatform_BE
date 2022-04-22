using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IHighSchoolService
    {
        Task<HighSchool> GetHighSchoolByCode(string highSchoolCode);

        Task<PageResult<HighSchoolFilterForSchoolAdmin>> GetAllHighSchools(HighSchoolFilterForSchoolAdmin filter, string sort, int page, int limit);
    }
    public partial class HighSchoolService
    {
        private readonly IConfigurationProvider _mapper;

        public HighSchoolService(IUnitOfWork unitOfWork, IHighSchoolRepository repository, IMapper mapper) : base(
            unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<HighSchool> GetHighSchoolByCode(string highSchoolCode)
        {
            var highSchool = await FirstOrDefaultAsyn(hs => hs.HighSchoolCode == highSchoolCode);
            if (highSchool == null)
            {
                throw new ErrorResponse((int)(HttpStatusCode.NotFound),
                    "Không thể tìm thấy trường THPT nào ứng với mã đã cung cấp.");
            }

            return highSchool;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<HighSchoolFilterForSchoolAdmin>> GetAllHighSchools(HighSchoolFilterForSchoolAdmin filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(h => h.DeletedAt == null)
                .ProjectTo<HighSchoolFilterForSchoolAdmin>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<HighSchoolFilterForSchoolAdmin>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}