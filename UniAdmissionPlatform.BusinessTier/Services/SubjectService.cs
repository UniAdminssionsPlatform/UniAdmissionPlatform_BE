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
using UniAdmissionPlatform.BusinessTier.Requests.Subject;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISubjectService
    {
        Task<PageResult<SubjectBaseViewModel>> GetAllSubject(SubjectBaseViewModel filter, string sort, int page, int limit);
        Task<SubjectBaseViewModel> GetSubjectById(int subjectId);
        Task<int> CreateSubject(CreateSubjectRequest createSubjectRequest);
        Task UpdateSubject(int subjectId, UpdateSubjectRequest updateSubjectRequest);
        Task DeleteSubjectById(int subjectId);

        Task<List<SubjectBaseViewModel>> GetBaseSubjects();
    }
    public partial class SubjectService
    {
        private readonly IConfigurationProvider _mapper;

        public SubjectService(IUnitOfWork unitOfWork, ISubjectRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SubjectBaseViewModel>> GetAllSubject(SubjectBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SubjectBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<SubjectBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<SubjectBaseViewModel> GetSubjectById(int subjectId)
        {
            var subject = await FirstOrDefaultAsyn(s => s.Id == subjectId);
            if (subject == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy môn học id = {subjectId}.");
            }
            
            return _mapper.CreateMapper().Map<SubjectBaseViewModel>(subject);
        }

        public async Task<int> CreateSubject(CreateSubjectRequest createSubjectRequest)
        {
            var subject = _mapper.CreateMapper().Map<Subject>(createSubjectRequest);
            await CreateAsyn(subject);
            
            return subject.Id;
        }

        public async Task UpdateSubject(int subjectId, UpdateSubjectRequest updateSubjectRequest)
        {
            var subject = await FirstOrDefaultAsyn(s => s.Id == subjectId);
            if (subject == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy môn học id = {subjectId}.");
            }
            subject.Name = updateSubjectRequest.Name;
            await UpdateAsyn(subject);
        }

        public async Task DeleteSubjectById(int subjectId)
        {
            var subject = await FirstOrDefaultAsyn(s => s.Id == subjectId);
            if (subject == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy môn học id = {subjectId}.");
            }
            await DeleteAsyn(subject);
        }

        public async Task<List<SubjectBaseViewModel>> GetBaseSubjects()
        {
            var strings = SubjectModule.BaseSubjectIds.Split(',');
            var subjectBaseViewModels = await Get().Where(s => strings.ToList().Select(int.Parse).Contains(s.Id)).ProjectTo<SubjectBaseViewModel>(_mapper).ToListAsync();
            return subjectBaseViewModels;
        }
    }
}