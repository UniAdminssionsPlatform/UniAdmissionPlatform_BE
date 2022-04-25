using System.Linq.Dynamic.Core;
using System.Net;
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
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IRoleService
    {
        Task<PageResult<RoleBaseViewModel>> GetAllRoles(RoleBaseViewModel filter, string sort,
            int page, int limit);

        Task<RoleBaseViewModel> GetRoleById(string id);
    }
    
    public partial class RoleService
    {
        private readonly IConfigurationProvider _mapper;
        
        public RoleService(IUnitOfWork unitOfWork, IRoleRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<RoleBaseViewModel>> GetAllRoles(RoleBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().ProjectTo<RoleBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<RoleBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<RoleBaseViewModel> GetRoleById(string id)
        {
            var role = await Get(r => r.Id == id).ProjectTo<RoleBaseViewModel>(_mapper).FirstOrDefaultAsync();
            if (role == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Chức vụ này không tồn tại.");
            }

            return role;
        } 
    }
}