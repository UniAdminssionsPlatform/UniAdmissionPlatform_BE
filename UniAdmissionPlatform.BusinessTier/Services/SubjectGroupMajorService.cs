using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroupMajor;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISubjectGroupMajorService
    {
        Task<PageResult<SubjectGroupMajorBaseViewModel>> GetSubjectGroupMajors(SubjectGroupMajorBaseViewModel filter,
            string sort, int page, int limit);

        Task CreateSubjectGroupMajor(CreateSubjectGroupMajorRequest createSubjectGroupMajorRequest);
        Task DeleteSubjectGroupMajor(int majorId, int subjectGroupId);
    }
    public partial class SubjectGroupMajorService
    {
        private readonly IConfigurationProvider _mapper;

        public SubjectGroupMajorService(IUnitOfWork unitOfWork, ISubjectGroupMajorRepository repository, IMapper mapper)
            : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 200;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SubjectGroupMajorBaseViewModel>> GetSubjectGroupMajors(SubjectGroupMajorBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SubjectGroupMajorBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<SubjectGroupMajorBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task CreateSubjectGroupMajor(CreateSubjectGroupMajorRequest createSubjectGroupMajorRequest)
        {
            var mapper = _mapper.CreateMapper();
            var subjectGroupMajorInRequest = mapper.Map<SubjectGroupMajor>(createSubjectGroupMajorRequest);

            var subjectGroupMajorInDb = await FirstOrDefaultAsyn(sgm => sgm.MajorId == subjectGroupMajorInRequest.MajorId && sgm.SubjectGroupId == subjectGroupMajorInRequest.SubjectGroupId);
            if (subjectGroupMajorInDb != null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Đã tồn tại trong hệ thống.");
            }

            await CreateAsyn(subjectGroupMajorInRequest);
        }

        public async Task DeleteSubjectGroupMajor(int majorId, int subjectGroupId)
        {
            var subjectGroupMajorInDb = await FirstOrDefaultAsyn(sgm => sgm.MajorId == majorId && sgm.SubjectGroupId == subjectGroupId);
            if (subjectGroupMajorInDb == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tồn tại trong hệ thống.");
            }
            
            await DeleteAsyn(subjectGroupMajorInDb);
        }
    }
}