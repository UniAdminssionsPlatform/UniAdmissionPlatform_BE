using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISchoolYearService
    {
        Task<PageResult<SchoolYearBaseViewModel>> GetAllSchoolYears(SchoolYearBaseViewModel filter, string sort,
            int page, int limit);
        Task<SchoolYearBaseViewModel> GetSchoolYearById(int schoolYearId);
    }
    
    public partial class SchoolYearService
    {
        private readonly IConfigurationProvider _mapper;
        
        public SchoolYearService(IUnitOfWork unitOfWork, ISchoolYearRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SchoolYearBaseViewModel>> GetAllSchoolYears(SchoolYearBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SchoolYearBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<SchoolYearBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<SchoolYearBaseViewModel> GetSchoolYearById(int schoolYearId)
        {
            var schoolYearById = await Get().Where(p => p.Id == schoolYearId)
                .ProjectTo<SchoolYearBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (schoolYearById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy năm học nào nào có id = {schoolYearId}");
            }
            return schoolYearById;
        }
    }
}