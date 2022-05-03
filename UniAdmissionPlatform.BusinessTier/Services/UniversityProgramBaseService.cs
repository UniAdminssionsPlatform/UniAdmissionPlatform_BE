using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityProgramService
    {
        Task<UniversityProgramBaseViewModel> GetUniversityProgramById(int universityProgramId);
        Task<PageResult<UniversityProgramBaseViewModel>> GetAllUniversityProgram(UniversityProgramBaseViewModel filter, string sort, int page, int limit);
        Task<int> CreateUniversityProgram(CreateUniversityProgramRequest createUniversityProgramRequest);
        Task UpdateUniversityProgram(int universityProgramId, UpdateUniversityProgramRequest updateUniversityProgramRequest);
        Task DeleteUniversityProgramById(int universityProgramId);
    }
    public partial class UniversityProgramService
    {
        private readonly IConfigurationProvider _mapper;

        public UniversityProgramService(IUnitOfWork unitOfWork, IUniversityProgramRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<UniversityProgramBaseViewModel> GetUniversityProgramById(int universityProgramId)
        {
            var universityProgramById = await Get()
                .Where(up => up.Id == universityProgramId && up.DeletedAt == null )
                .ProjectTo<UniversityProgramBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (universityProgramById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy chương trình học nào có id = {universityProgramId}");
            }
            return universityProgramById;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<UniversityProgramBaseViewModel>> GetAllUniversityProgram(UniversityProgramBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<UniversityProgramBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UniversityProgramBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<int> CreateUniversityProgram(CreateUniversityProgramRequest createUniversityProgramRequest)
        {
            var majorDepartment = _mapper.CreateMapper().Map<UniversityProgram>(createUniversityProgramRequest);
            
            majorDepartment.CreatedAt = DateTime.Now;
            majorDepartment.UpdatedAt = DateTime.Now;
            
            await CreateAsyn(majorDepartment);
            return majorDepartment.Id;
        }

        public async Task UpdateUniversityProgram(int universityProgramId, UpdateUniversityProgramRequest updateUniversityProgramRequest)
        {
            var majorDepartment = await FirstOrDefaultAsyn(up => up.Id == universityProgramId && up.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chương trình học id:{universityProgramId}.");
            }
            var mapper = _mapper.CreateMapper();
            majorDepartment = mapper.Map(updateUniversityProgramRequest,majorDepartment);
            majorDepartment.UpdatedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }

        public async Task DeleteUniversityProgramById(int universityProgramId)
        {
            var majorDepartment = await FirstOrDefaultAsyn(up => up.Id == universityProgramId && up.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chương trình học id:{universityProgramId}.");
            }
            majorDepartment.UpdatedAt = DateTime.Now;
            majorDepartment.DeletedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }
    }
}