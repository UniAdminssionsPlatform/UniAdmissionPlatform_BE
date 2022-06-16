using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Apis.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IStudentRecordItemService
    {
        Task<PageResult<StudentRecordItemBaseViewModel>> GetAllStudentRecordItem(StudentRecordItemBaseViewModel filter,
            int? studentId, string sort, int page, int limit);

        Task<int> CreateStudentRecordItem(CreateStudentRecordItemRequest createStudentRecordItemRequest,
            int studentId = 0);
        Task UpdateStudentRecordItem(int studentRecordItemId, UpdateStudentRecordItemRequest updateStudentRecordItemRequest, int studentId = 0);
        Task<StudentRecordItemBaseViewModel> GetStudentRecordItemById(int studentRecordItemId);
    }
    public partial class StudentRecordItemService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly ISchoolRecordRepository _schoolRecordRepository;

        public StudentRecordItemService(IUnitOfWork unitOfWork, IStudentRecordItemRepository repository, IMapper mapper, ISchoolRecordRepository schoolRecordRepository) : base(unitOfWork,
            repository)
        {
            _schoolRecordRepository = schoolRecordRepository;
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<StudentRecordItemBaseViewModel>> GetAllStudentRecordItem(StudentRecordItemBaseViewModel filter, int? studentId,string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(sri => studentId == null || sri.SchoolRecord.StudentId == studentId)
                .ProjectTo<StudentRecordItemBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<StudentRecordItemBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        

        public async Task<int> CreateStudentRecordItem(CreateStudentRecordItemRequest createStudentRecordItemRequest, int studentId = 0)
        {
            if (studentId != 0 && !await _schoolRecordRepository.Get()
                    .Where(sr => sr.StudentId == studentId && sr.Id == createStudentRecordItemRequest.SchoolRecordId)
                    .AnyAsync())
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy học bạ.");
            }
            
            var studentRecordItem = _mapper.CreateMapper().Map<StudentRecordItem>(createStudentRecordItemRequest);

            await CreateAsyn(studentRecordItem);
            return studentRecordItem.Id;
        }

        public async Task UpdateStudentRecordItem(int studentRecordItemId, UpdateStudentRecordItemRequest updateStudentRecordItemRequest, int studentId)
        {
            var sr = await _schoolRecordRepository
                .Get()
                .Where(sr => sr.Id == updateStudentRecordItemRequest.SchoolRecordId)
                .FirstOrDefaultAsync();
            if (sr.StudentId != studentId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy học bạ.");
            }
            
            var studentRecordItem = await FirstOrDefaultAsyn(s => s.Id == studentRecordItemId);
            if (studentRecordItem == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thông tin điểm id:{studentRecordItemId}.");
            }
            var mapper = _mapper.CreateMapper();
            studentRecordItem = mapper.Map(updateStudentRecordItemRequest,studentRecordItem);
            await UpdateAsyn(studentRecordItem);
        }
        
        public async Task<StudentRecordItemBaseViewModel> GetStudentRecordItemById(int studentRecordItemId)
        {
            var stuRecordItemId = await Get().Where(u => u.Id == studentRecordItemId)
                .ProjectTo<StudentRecordItemBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (stuRecordItemId == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy thông tin điểm nào có id = {studentRecordItemId}");
            }

            return stuRecordItemId;
        }
        
    }
}