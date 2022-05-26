using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroup;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISubjectGroupService
    {
        Task<SubjectGroupBaseViewModel> GetById(int id);

        Task<PageResult<SubjectGroupBaseViewModel>> GetAllSubjectGroups(SubjectGroupBaseViewModel filter, string sort,
            int page,
            int limit);

        Task<int> CreateSubjectGroup(CreateSubjectGroupRequest createSubjectGroupRequest);
        Task UpdateSubjectGroup(int id, UpdateSubjectGroupRequest updateSubjectGroupRequest);

        Task DeleteSubjectGroup(int id);

        SchoolRecordBaseViewModel GetScoreOfStudent(int id, int schoolYearId, int studentId);
    }

    public partial class SubjectGroupService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly ISchoolRecordRepository _schoolRecordRepository;

        public SubjectGroupService(IUnitOfWork unitOfWork, ISubjectGroupRepository repository, IMapper mapper,
            ISchoolRecordRepository schoolRecordRepository) : base(unitOfWork,
            repository)
        {
            _schoolRecordRepository = schoolRecordRepository;
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 200;
        private const int DefaultPaging = 100;

        public async Task<SubjectGroupBaseViewModel> GetById(int id)
        {
            var subjectGroup = await FirstOrDefaultAsyn(sg => sg.Id == id);
            if (subjectGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không thể tìm thấy nhóm ngành với id = {id}");
            }

            return _mapper.CreateMapper().Map<SubjectGroupBaseViewModel>(subjectGroup);
        }

        public async Task<PageResult<SubjectGroupBaseViewModel>> GetAllSubjectGroups(SubjectGroupBaseViewModel filter,
            string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SubjectGroupBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<SubjectGroupBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<int> CreateSubjectGroup(CreateSubjectGroupRequest createSubjectGroupRequest)
        {
            var mapper = _mapper.CreateMapper();
            var subjectGroup = mapper.Map<SubjectGroup>(createSubjectGroupRequest);

            await CreateAsyn(subjectGroup);

            return subjectGroup.Id;
        }

        public async Task UpdateSubjectGroup(int id, UpdateSubjectGroupRequest updateSubjectGroupRequest)
        {
            var subjectGroup = await FirstOrDefaultAsyn(sg => sg.Id == id);
            if (subjectGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không thể tìm thấy nhóm ngành với id = {id}");
            }

            var mapper = _mapper.CreateMapper();
            mapper.Map(updateSubjectGroupRequest, subjectGroup);

            await UpdateAsyn(subjectGroup);
        }


        public async Task DeleteSubjectGroup(int id)
        {
            var subjectGroup = await FirstOrDefaultAsyn(sg => sg.Id == id);
            if (subjectGroup == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không thể tìm thấy nhóm ngành với id = {id}");
            }

            await DeleteAsyn(subjectGroup);
        }

        public SchoolRecordBaseViewModel GetScoreOfStudent(int id, int schoolYearId, int studentId)
        {
            var subjectGroupSubjects = Get().Include(sg => sg.SubjectGroupSubjects).ThenInclude(sgs => sgs.Subject).FirstOrDefault(sg => sg.Id == id);
            if (subjectGroupSubjects == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy khối thi.");
            }

            var schoolRecord = _schoolRecordRepository.Get().Include(sr => sr.StudentRecordItems.Where(sri =>
                    subjectGroupSubjects.SubjectGroupSubjects.Select(sgs => sgs.SubjectId).Contains(sri.SubjectId)))
                .FirstOrDefault(sr => sr.SchoolYearId == schoolYearId && sr.StudentId == studentId);
            if (schoolRecord == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy học bạ.");
            }

            return _mapper.CreateMapper().Map<SchoolRecordWithStudentRecordItemModel>(schoolRecord);
        }
    }
}