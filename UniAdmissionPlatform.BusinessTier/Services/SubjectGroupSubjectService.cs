using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroupSubject;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISubjectGroupSubjectService
    {
        Task<PageResult<SubjectGroupSubjectBaseViewModel>> GetSubjectGroupSubjects(
            SubjectGroupSubjectBaseViewModel filter, string sort, int page, int limit);

        Task CreateSubjectGroupSubject(CreateSubjectGroupSubjectRequest createSubjectGroupSubjectRequest);
        Task DeleteSubjectGroupSubject(int subjectGroupId, int subjectId);
    }
    public partial class SubjectGroupSubjectService
    {
        private readonly IConfigurationProvider _mapper;

        public SubjectGroupSubjectService(IUnitOfWork unitOfWork, ISubjectGroupSubjectRepository repository,
            IMapper mapper) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 200;
        private const int DefaultPaging = 100;

        public async Task<PageResult<SubjectGroupSubjectBaseViewModel>> GetSubjectGroupSubjects(SubjectGroupSubjectBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SubjectGroupSubjectBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<SubjectGroupSubjectBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task CreateSubjectGroupSubject(CreateSubjectGroupSubjectRequest createSubjectGroupSubjectRequest)
        {
            var mapper = _mapper.CreateMapper();
            var subjectGroupSubjectInRequest = mapper.Map<CreateSubjectGroupSubjectRequest, SubjectGroupSubject>(createSubjectGroupSubjectRequest);

            var subjectGroupSubjectInDb = await FirstOrDefaultAsyn(sgs =>
                sgs.SubjectId == subjectGroupSubjectInRequest.SubjectId &&
                sgs.SubjectGroupId == subjectGroupSubjectInRequest.SubjectGroupId);
            if (subjectGroupSubjectInDb != null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Đã tồn tại trong hệ thống.");
            }

            await CreateAsyn(subjectGroupSubjectInRequest);
        }

        public async Task DeleteSubjectGroupSubject(int subjectGroupId, int subjectId)
        {

            var subjectGroupSubjectInDb = await FirstOrDefaultAsyn(sgs =>
                sgs.SubjectId == subjectId &&
                sgs.SubjectGroupId == subjectGroupId);
            if (subjectGroupSubjectInDb == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tồn tại trong hệ thống.");
            }

            await DeleteAsyn(subjectGroupSubjectInDb);
        }
    }
}