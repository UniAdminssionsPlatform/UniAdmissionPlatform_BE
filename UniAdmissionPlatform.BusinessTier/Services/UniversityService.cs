using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityService
    {
        Task<UniversityCodeViewModel> GetUniversityNameByCode(string highSchoolCode);
        Task<PageResult<UniversityBaseViewModel>> GetAllUniversities(UniversityBaseViewModel filter, string sort, int page, int limit);
        
    }
    public partial class UniversityService
    {
        private readonly IConfigurationProvider _mapper;

        public UniversityService(IUnitOfWork unitOfWork, IUniversityRepository repository, IMapper mapper) : base(
            unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<UniversityCodeViewModel> GetUniversityNameByCode(string universityCode)
        {
            var university = await Get().ProjectTo<UniversityCodeViewModel>(_mapper).FirstOrDefaultAsync(u => u.UniversityCode == universityCode);
            if (university == null)
            {
                throw new ErrorResponse((int)(HttpStatusCode.NotFound),
                    "Không thể tìm thấy trường đại học nào ứng với mã đã cung cấp.");
            }

            return university;
        }
        
         
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<UniversityBaseViewModel>> GetAllUniversities(UniversityBaseViewModel filter, string sort, int page, int limit)
        {
            int statusU = 1; //status Active
            var (total, queryable) = Get
                    (s => s.Status == statusU)
                .ProjectTo<UniversityBaseViewModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UniversityBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
    }
}