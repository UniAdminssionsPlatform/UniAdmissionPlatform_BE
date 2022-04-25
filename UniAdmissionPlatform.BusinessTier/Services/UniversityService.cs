using System.Linq.Dynamic.Core;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.University;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityService
    {
        Task<UniversityCodeViewModel> GetUniversityNameByCode(string highSchoolCode);
        Task<PageResult<UniversityBaseViewModel>> GetAllUniversities(UniversityBaseViewModel filter, string sort, int page, int limit);
        Task<UniversityBaseViewModel> GetUniversityByID(int Id);

        Task<int> CreateUniversity(CreateUniversityRequest createUniversityRequest);
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
                throw new ErrorResponse(StatusCodes.Status404NotFound,
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
        
        public async Task<UniversityBaseViewModel> GetUniversityByID(int universityId)
        {
            int statusU = 1; //status Active
            var universityById = await Get().Where(u => u.Id == universityId && u.Status == statusU)
                .ProjectTo<UniversityBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (universityById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy trường đại học nào có id = {universityId}");
            }

            return universityById;
        }

        public async Task<int> CreateUniversity(CreateUniversityRequest createUniversityRequest)
        {
            var mapper = _mapper.CreateMapper();
            var university = mapper.Map<University>(createUniversityRequest);
            
            

            await CreateAsyn(university);
            return university.Id;
        }
    }
}