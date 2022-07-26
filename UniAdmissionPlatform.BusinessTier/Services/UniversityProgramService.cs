using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Constants;
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

        Task<PageResult<UniversityProgramBaseViewModel>> GetAllUniversityProgram(UniversityProgramBaseViewModel filter,
            string sort, int page, int limit);

        Task<int> CreateUniversityProgram(int universityId,
            CreateUniversityProgramRequest createUniversityProgramRequest);

        Task UpdateUniversityProgram(int universityProgramId,
            UpdateUniversityProgramRequest updateUniversityProgramRequest);

        Task DeleteUniversityProgramById(int universityProgramId);

        Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>> GetAllUniversityProgramWithDetail(
            UniversityProgramWithMajorDepartmentAndSchoolYearModel filter, string sort, int page, int limit);

        Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>>
            GetAllUniversityProgramWithDetailByUniversityId(int universityId, string sort, int page, int limit);

        Task<ListUniversityProgramAdmission> GetUniversityAdmissionProgram(int universityId, int schoolYearId);

        Task<ListUniversityProgramAdmissionForStudent> GetUniversityAdmissionProgramByStudentId(int studentId);
        Task<PageResult<UniversityProgramDetailViewModel>> GetAll(UniversityProgramBaseViewModel filter, int universityId, int page, int limit);
    }

    public partial class UniversityProgramService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IMajorDepartmentRepository _majorDepartmentRepository;
        private readonly ISchoolRecordRepository _schoolRecordRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IGroupPointRepository _groupPointRepository;

        public UniversityProgramService(IUnitOfWork unitOfWork, IUniversityProgramRepository repository, IMapper mapper,
            IMajorDepartmentRepository majorDepartmentRepository, ISchoolRecordRepository schoolRecordRepository,
            ISubjectRepository subjectRepository, IGroupPointRepository groupPointRepository) : base(unitOfWork,
            repository)
        {
            _majorDepartmentRepository = majorDepartmentRepository;
            _mapper = mapper.ConfigurationProvider;
            _schoolRecordRepository = schoolRecordRepository;
            _subjectRepository = subjectRepository;
            _groupPointRepository = groupPointRepository;
        }

        public async Task<PageResult<UniversityProgramDetailViewModel>> GetAll(UniversityProgramBaseViewModel filter, int universityId, int page, int limit)
        {
            var (total, universityProgramBaseViewModels) = Get()
                .Where(up =>
                    up.MajorDepartment.University.Id == universityId && up.DeletedAt == null)
                .Where(up => filter.MajorDepartmentId == null || up.MajorDepartmentId == filter.MajorDepartmentId)
                .Where(up => filter.SchoolYearId == null || up.SchoolYearId == filter.SchoolYearId)
                .Where(up => filter.SubjectGroupId == null || up.SubjectGroupId == filter.SubjectGroupId)
                .ProjectTo<UniversityProgramDetailViewModel>(_mapper)
                .PagingIQueryable(page, limit, 20, 1);

            return new PageResult<UniversityProgramDetailViewModel>
            {
                Limit = limit,
                List = await universityProgramBaseViewModels.ToListAsync(),
                Page = page,
                Total = total
            };
        }

        public async Task<ListUniversityProgramAdmission> GetUniversityAdmissionProgram(int universityId,
            int schoolYearId)
        {
            var universityProgramAdmissions = await Get()
                .Where(up =>
                    up.MajorDepartment.University.Id == universityId && up.DeletedAt == null &&
                    up.SchoolYearId == schoolYearId)
                .ProjectTo<UniversityProgramAdmission>(_mapper)
                .ToListAsync();
            return new ListUniversityProgramAdmission(universityProgramAdmissions);
        }

        public async Task<ListUniversityProgramAdmissionForStudent> GetUniversityAdmissionProgramByStudentId(int studentId)
        {
            var mainSubjectIds = SubjectModule.BaseSubjectIds.Split(',');
            var year = DateTime.Now.Year;
            var schoolRecord = await _schoolRecordRepository.Get().Include(sr => sr.StudentRecordItems.Where(sri => mainSubjectIds.Contains(sri.SubjectId.ToString()))).FirstOrDefaultAsync(sr => sr.SchoolYear.Year == year && sr.StudentId == studentId);
            if (schoolRecord == null)
            {
                return null;
            }

            if (schoolRecord.StudentRecordItems.Count(sri => sri.Score != null) != mainSubjectIds.Length)
            {
                return null;
            }
            
            var sumOfPoint = 0.0;
            foreach (var schoolRecordStudentRecordItem in schoolRecord.StudentRecordItems)
            {
                sumOfPoint += schoolRecordStudentRecordItem.Score ?? 0;
            }



            var averageOfPoint = sumOfPoint / schoolRecord.StudentRecordItems.Count;

            var groupPoints = await _groupPointRepository.Get().Where(gp => gp.Point != null && gp.Point <= averageOfPoint + 1).Select(gp => gp.Id).ToListAsync();

            var universityProgramAdmissions = await Get().Where(up => groupPoints.Contains(up.GroupPointId ?? 0) && up.SchoolYear.Year == year - 1).ProjectTo<UniversityProgramAdmission>(_mapper).ToListAsync();
            return new ListUniversityProgramAdmissionForStudent(universityProgramAdmissions);
        }

        public async Task<UniversityProgramBaseViewModel> GetUniversityProgramById(int universityProgramId)
        {
            var universityProgramById = await Get()
                .Where(up => up.Id == universityProgramId && up.DeletedAt == null)
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

        public async Task<PageResult<UniversityProgramBaseViewModel>> GetAllUniversityProgram(
            UniversityProgramBaseViewModel filter, string sort, int page, int limit)
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

        public async Task<int> CreateUniversityProgram(int universityId,
            CreateUniversityProgramRequest createUniversityProgramRequest)
        {
            var majorDepartment = _majorDepartmentRepository.Get().FirstOrDefault(md => md.Id == createUniversityProgramRequest.MajorDepartmentId && md.UniversityId == universityId);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Bạn không có quyền.");
            }
            var mapper = _mapper.CreateMapper();
            var universityProgram = mapper.Map<UniversityProgram>(createUniversityProgramRequest);
            await CreateAsyn(universityProgram);
            return universityProgram.Id;
        }

        public async Task UpdateUniversityProgram(int universityProgramId,
            UpdateUniversityProgramRequest updateUniversityProgramRequest)
        {
            var majorDepartment = await FirstOrDefaultAsyn(up => up.Id == universityProgramId && up.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    $"Không tìm thấy chương trình học id:{universityProgramId}.");
            }

            var mapper = _mapper.CreateMapper();
            majorDepartment = mapper.Map(updateUniversityProgramRequest, majorDepartment);
            majorDepartment.UpdatedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }

        public async Task DeleteUniversityProgramById(int universityProgramId)
        {
            var majorDepartment = await FirstOrDefaultAsyn(up => up.Id == universityProgramId && up.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    $"Không tìm thấy chương trình học id:{universityProgramId}.");
            }

            majorDepartment.UpdatedAt = DateTime.Now;
            majorDepartment.DeletedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }

        public async Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>>
            GetAllUniversityProgramWithDetail(UniversityProgramWithMajorDepartmentAndSchoolYearModel filter,
                string sort, int page, int limit)
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

        public async Task<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>>
            GetAllUniversityProgramWithDetailByUniversityId(int universityId, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(up => up.MajorDepartment.UniversityId == universityId && up.DeletedAt == null)
                .ProjectTo<UniversityProgramWithMajorDepartmentAndSchoolYearModel>(_mapper)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            ;

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