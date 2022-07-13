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
        Task<int> CreateUniversityProgram(int universityId, BulkCreateUniversityProgramMajorRequest bulkCreateUniversityProgramMajorRequest);
        Task UpdateUniversityProgram(int universityProgramId, UpdateUniversityProgramRequest updateUniversityProgramRequest);
        Task DeleteUniversityProgramById(int universityProgramId);

        Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>> GetAllUniversityProgramWithDetail(
            UniversityProgramWithMajorDepartmentAndSchoolYearModel filter, string sort, int page, int limit);

        Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>> GetAllUniversityProgramWithDetailByUniversityId(int universityId, string sort, int page, int limit);

        Task<ListUniversityProgramAdmission> GetUniversityAdmissionProgram(int universityId, int schoolYearId);
    }
    public partial class UniversityProgramService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IMajorDepartmentRepository _majorDepartmentRepository;

        public UniversityProgramService(IUnitOfWork unitOfWork, IUniversityProgramRepository repository, IMapper mapper, IMajorDepartmentRepository majorDepartmentRepository) : base(unitOfWork,
            repository)
        {
            _majorDepartmentRepository = majorDepartmentRepository;
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<ListUniversityProgramAdmission> GetUniversityAdmissionProgram(int universityId, int schoolYearId)
        {
            var universityProgramAdmissions = await Get()
                .Where(up => up.MajorDepartment.University.Id == universityId && up.DeletedAt == null && up.SchoolYearId == schoolYearId)
                .ProjectTo<UniversityProgramAdmission>(_mapper)
                .ToListAsync();
            return new ListUniversityProgramAdmission(universityProgramAdmissions);
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
                .Where(up => up.DeletedAt == null)
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
        
        public async Task<int> CreateUniversityProgram(int universityId, BulkCreateUniversityProgramMajorRequest bulkCreateUniversityProgramMajorRequest)
        {
            var majorDepartments = await _majorDepartmentRepository.Get().Where(md =>
                md.UniversityId == universityId && bulkCreateUniversityProgramMajorRequest.MajorDepartmentDetails
                    .Select(mdd => mdd.MajorDepartmentId).Contains(md.Id)).CountAsync();

            if (majorDepartments != bulkCreateUniversityProgramMajorRequest.MajorDepartmentDetails.Count)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Một số ngành không có ở trường bạn.");
            }

            var createUniversityProgramRequests = bulkCreateUniversityProgramMajorRequest.ToUniversityProgramRequests();
            foreach (var createUniversityProgramRequest in createUniversityProgramRequests)
            {
                var majorDepartment = _mapper.CreateMapper().Map<UniversityProgram>(createUniversityProgramRequest);
            
                majorDepartment.CreatedAt = DateTime.Now;
                majorDepartment.UpdatedAt = DateTime.Now;
            
                await CreateAsyn(majorDepartment);
            }
            
            return bulkCreateUniversityProgramMajorRequest.MajorDepartmentDetails.Count;
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
        
        public async Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>> GetAllUniversityProgramWithDetail(UniversityProgramWithMajorDepartmentAndSchoolYearModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(u => u.DeletedAt == null)
                .ProjectTo<UniversityProgramWithMajorDepartmentAndSchoolYearModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>> GetAllUniversityProgramWithDetailByUniversityId(int universityId, string sort, int page, int limit)
        {
            var (total, queryable) = Get().Where(up => up.MajorDepartment.UniversityId == universityId && up.DeletedAt == null).ProjectTo<UniversityProgramWithMajorDepartmentAndSchoolYearModel>(_mapper).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);;
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}