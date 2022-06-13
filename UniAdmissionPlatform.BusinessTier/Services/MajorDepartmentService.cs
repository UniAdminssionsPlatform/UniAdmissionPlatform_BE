using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.MajorDepartment;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IMajorDepartmentService
    {
        Task<PageResult<MajorDepartmentBaseViewModel>> GetAllMajorDepartment(MajorDepartmentBaseViewModel filter, string sort, int page, int limit);
        Task<int> CreateMajorDepartment(int universityId, CreateMajorDepartmentRequest createMajorDepartmentRequest);
        Task UpdateMajorDepartment(int majorDepartmentId, UpdateMajorDepartmentRequest updateMajorDepartmentRequest);
        Task DeleteMajorDepartmentById(int majorDepartmentId);
    }
    public partial class MajorDepartmentService
    {
        private readonly IConfigurationProvider _mapper;

        public MajorDepartmentService(IUnitOfWork unitOfWork, IMajorDepartmentRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<MajorDepartmentBaseViewModel>> GetAllMajorDepartment(MajorDepartmentBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(m => m.DeletedAt == null)
                .ProjectTo<MajorDepartmentBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<MajorDepartmentBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        

        public async Task<int> CreateMajorDepartment(int universityId, CreateMajorDepartmentRequest createMajorDepartmentRequest)
        {
            var majorDepartment = _mapper.CreateMapper().Map<MajorDepartment>(createMajorDepartmentRequest);
            majorDepartment.UniversityId = universityId ;
            majorDepartment.CreatedAt = DateTime.Now;
            majorDepartment.UpdatedAt = DateTime.Now;
            
            await CreateAsyn(majorDepartment);
            return majorDepartment.Id;
        }

        public async Task UpdateMajorDepartment(int majorDepartmentId, UpdateMajorDepartmentRequest updateMajorDepartmentRequest)
        {
            var majorDepartment = await FirstOrDefaultAsyn(md => md.Id == majorDepartmentId && md.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy phòng ban chuyên ngành học id:{majorDepartmentId}.");
            }
            var mapper = _mapper.CreateMapper();
            majorDepartment = mapper.Map(updateMajorDepartmentRequest,majorDepartment);
            majorDepartment.UpdatedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }

        public async Task DeleteMajorDepartmentById(int majorDepartmentId)
        {
            var majorDepartment = await FirstOrDefaultAsyn(md => md.Id == majorDepartmentId && md.DeletedAt == null);
            if (majorDepartment == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy phòng ban chuyên ngành học id:{majorDepartmentId}.");
            }
            majorDepartment.UpdatedAt = DateTime.Now;
            majorDepartment.DeletedAt = DateTime.Now;
            await UpdateAsyn(majorDepartment);
        }
    }
}