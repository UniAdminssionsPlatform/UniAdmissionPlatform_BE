using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmission;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IGoalAdmissionService
    {
        Task<int> CreateGoalAdmission(CreateGoalAdmissionRequest createGoalAdmissionRequest);
        Task UpdateGoalAdmission(int goalAdmissionId, UpdateGoalAdmissionRequest updateGoalAdmissionRequest);
        Task DeleteGoalAdmission(int goalAdmissionId);
        Task<PageResult<GoalAdmissionBaseViewModel>> GetAllGoalAdmissions(GoalAdmissionBaseViewModel filter, string sort,
            int page, int limit);
        Task<GoalAdmissionBaseViewModel> GetGoalAdmissionById(int goalAdmissionId);
    }
    
    public partial class GoalAdmissionService
    {
        private readonly IConfigurationProvider _mapper;
        
        public GoalAdmissionService(IUnitOfWork unitOfWork, IGoalAdmissionRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateGoalAdmission(CreateGoalAdmissionRequest createGoalAdmissionRequest)
        {
            var mapper = _mapper.CreateMapper();
            var goalAdmission = mapper.Map<GoalAdmission>(createGoalAdmissionRequest);
            
            goalAdmission.CreatedAt = DateTime.Now;
            goalAdmission.UpdatedAt = DateTime.Now;
            
            await CreateAsyn(goalAdmission);
            return goalAdmission.Id;
        }

        public async Task UpdateGoalAdmission(int goalAdmissionId, UpdateGoalAdmissionRequest updateGoalAdmissionRequest)
        {
            var goalAdmission = await Get().Where(g => g.Id == goalAdmissionId && g.DeletedAt == null).FirstOrDefaultAsync();
            if (goalAdmission == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy goalAdmission với goalAdmissionId = {goalAdmissionId}");
            }

            var mapper = _mapper.CreateMapper();
            goalAdmission = mapper.Map(updateGoalAdmissionRequest, goalAdmission);
            
            goalAdmission.UpdatedAt = DateTime.Now;
            await UpdateAsyn(goalAdmission);
        }

        public async Task DeleteGoalAdmission(int goalAdmissionId)
        {
            var goalAdmission = await Get().Where(g => g.Id == goalAdmissionId && g.DeletedAt == null).FirstOrDefaultAsync();
            if (goalAdmission == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy goalAdmission với goalAdmissionId = {goalAdmissionId}");
            }
            
            goalAdmission.DeletedAt = DateTime.Now;
            
            await UpdateAsyn(goalAdmission);
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<GoalAdmissionBaseViewModel>> GetAllGoalAdmissions(GoalAdmissionBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(g => g.DeletedAt == null).ProjectTo<GoalAdmissionBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<GoalAdmissionBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<GoalAdmissionBaseViewModel> GetGoalAdmissionById(int goalAdmissionId)
        {
            var goalAdmissionById = await Get().Where(g => g.Id == goalAdmissionId && g.DeletedAt == null)
                .ProjectTo<GoalAdmissionBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (goalAdmissionById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy goalAdmission nào nào có goalAdmissionId = {goalAdmissionId}");
            }
            return goalAdmissionById;
        }
    }
}