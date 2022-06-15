using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmissionType;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IGoalAdmissionTypeService
    {
        Task<int> CreateGoalAdmissionType(CreateGoalAdmissionTypeRequest createGoalAdmissionTypeRequest);
        Task UpdateGoalAdmissionType(int goalAdmissionTypeId, UpdateGoalAdmissionTypeRequest updateGoalAdmissionTypeRequest);
        Task DeleteGoalAdmissionType(int goalAdmissionTypeId);
        Task<PageResult<GoalAdmissionTypeBaseViewModel>> GetAllGoalAdmissionTypeTypes(GoalAdmissionTypeBaseViewModel filter, string sort,
            int page, int limit);
        Task<GoalAdmissionTypeBaseViewModel> GetGoalAdmissionTypeById(int goalAdmissionTypeId);
    }
    
    public partial class GoalAdmissionTypeService
    {
        private readonly IConfigurationProvider _mapper;
        
        public GoalAdmissionTypeService(IUnitOfWork unitOfWork, IGoalAdmissionTypeRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateGoalAdmissionType(CreateGoalAdmissionTypeRequest createGoalAdmissionTypeRequest)
        {
            var mapper = _mapper.CreateMapper();
            var goalAdmissionType = mapper.Map<GoalAdmissionType>(createGoalAdmissionTypeRequest);
            
            goalAdmissionType.CreatedAt = DateTime.Now;
            goalAdmissionType.UpdatedAt = DateTime.Now;
            
            await CreateAsyn(goalAdmissionType);
            return goalAdmissionType.Id;
        }

        public async Task UpdateGoalAdmissionType(int goalAdmissionTypeId, UpdateGoalAdmissionTypeRequest updateGoalAdmissionTypeRequest)
        {
            var goalAdmissionType = await Get().Where(g => g.Id == goalAdmissionTypeId && g.DeletedAt == null).FirstOrDefaultAsync();
            if (goalAdmissionType == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy goalAdmissionType với goalAdmissionTypeId = {goalAdmissionTypeId}");
            }

            var mapper = _mapper.CreateMapper();
            goalAdmissionType = mapper.Map(updateGoalAdmissionTypeRequest, goalAdmissionType);
            
            goalAdmissionType.UpdatedAt = DateTime.Now;
            await UpdateAsyn(goalAdmissionType);
        }

        public async Task DeleteGoalAdmissionType(int goalAdmissionTypeId)
        {
            var goalAdmissionType = await Get().Where(g => g.Id == goalAdmissionTypeId && g.DeletedAt == null).FirstOrDefaultAsync();
            if (goalAdmissionType == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy goalAdmissionType với goalAdmissionTypeId = {goalAdmissionTypeId}");
            }
            
            goalAdmissionType.DeletedAt = DateTime.Now;
            
            await UpdateAsyn(goalAdmissionType);
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<GoalAdmissionTypeBaseViewModel>> GetAllGoalAdmissionTypeTypes(GoalAdmissionTypeBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(g => g.DeletedAt == null).ProjectTo<GoalAdmissionTypeBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<GoalAdmissionTypeBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<GoalAdmissionTypeBaseViewModel> GetGoalAdmissionTypeById(int goalAdmissionTypeId)
        {
            var goalAdmissionById = await Get().Where(g => g.Id == goalAdmissionTypeId && g.DeletedAt == null)
                .ProjectTo<GoalAdmissionTypeBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (goalAdmissionById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy goalAdmissionType nào nào có goalAdmissionTypeId = {goalAdmissionTypeId}");
            }
            return goalAdmissionById;
        }
    }
}