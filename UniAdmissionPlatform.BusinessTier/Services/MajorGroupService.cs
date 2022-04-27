using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.MajorGroup;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IMajorGroupService
    {
        Task<PageResult<MajorGroupBaseViewModel>> GetAllMajorGroup(MajorGroupBaseViewModel filter, string sort,
            int page, int limit);
        Task<MajorGroupBaseViewModel> GetMajorGroupById(int id);
        Task<int> CreateMajorGroup(CreateMajorGroupRequest createMajorGroupRequest);
        Task UpdateMajorGroup(int id, UpdateMajorGroupRequest updateMajorGroupRequest);
        Task DeleteMajorGroup(int id);
    }
    
    public partial class MajorGroupService
    {
        private readonly IConfigurationProvider _mapper;

        public MajorGroupService(IUnitOfWork unitOfWork, IMajorGroupRepository repository, IMapper mapper) : base(
            unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;
        
        public async Task<PageResult<MajorGroupBaseViewModel>> GetAllMajorGroup(MajorGroupBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<MajorGroupBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<MajorGroupBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<MajorGroupBaseViewModel> GetMajorGroupById(int id)
        {
            var majorGroup = await FirstOrDefaultAsyn(mg => mg.Id == id);

            if (majorGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy nhóm ngành.");
            }

            return _mapper.CreateMapper().Map<MajorGroupBaseViewModel>(majorGroup);
        }

        public async Task<int> CreateMajorGroup(CreateMajorGroupRequest createMajorGroupRequest)
        {
            var majorGroup = _mapper.CreateMapper().Map<MajorGroup>(createMajorGroupRequest);

            await CreateAsyn(majorGroup);
            return majorGroup.Id;
        }

        public async Task UpdateMajorGroup(int id, UpdateMajorGroupRequest updateMajorGroupRequest)
        {
            var majorGroup = await FirstOrDefaultAsyn(mg => mg.Id == id);

            if (majorGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy nhóm ngành.");
            }

            majorGroup.Name = updateMajorGroupRequest.Name;

            await UpdateAsyn(majorGroup);
        }

        public async Task DeleteMajorGroup(int id)
        {
            var majorGroup = await Get().Include(mg => mg.Majors).FirstOrDefaultAsync(mg => mg.Id == id);

            if (majorGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy nhóm ngành.");
            }

            if (majorGroup.Majors == null || majorGroup.Majors.Any())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Nhóm ngành đang chứa các ngành.");
            }

            await DeleteAsyn(majorGroup);
        }
    }
}