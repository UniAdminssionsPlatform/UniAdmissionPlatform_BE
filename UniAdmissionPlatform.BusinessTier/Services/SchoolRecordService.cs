using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISchoolRecordService
    {
        Task<PageResult<SchoolRecordBaseViewModel>> GetAllSchoolRecord(SchoolRecordBaseViewModel filter, string sort, int page, int limit);
        Task<int> CreateSchoolRecord(int studentId, CreateSchoolRecordRequest createSchoolRecordRequest);
        Task UpdateSchoolRecord(int schoolRecordId, int studentId, UpdateSchoolRecordRequest updateSchoolRecordRequest);
        Task DeleteSchoolRecordById(int schoolRecordId, int studentId);
    }
    public partial class SchoolRecordService
    {
        private readonly IConfigurationProvider _mapper;

        public SchoolRecordService(IUnitOfWork unitOfWork, ISchoolRecordRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SchoolRecordBaseViewModel>> GetAllSchoolRecord(SchoolRecordBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<SchoolRecordBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<SchoolRecordBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        

        public async Task<int> CreateSchoolRecord(int studentId, CreateSchoolRecordRequest createSchoolRecordRequest)
        {
            var schoolRecord = _mapper.CreateMapper().Map<SchoolRecord>(createSchoolRecordRequest);
            schoolRecord.StudentId = studentId;

            await CreateAsyn(schoolRecord);
            return schoolRecord.Id;
        }

        public async Task UpdateSchoolRecord(int schoolRecordId, int studentId, UpdateSchoolRecordRequest updateSchoolRecordRequest)
        {
            var schoolRecord = await FirstOrDefaultAsyn(sr => sr.Id == schoolRecordId && sr.Student.Id == studentId);
            if (schoolRecord == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy phiếu điểm(School Record) id:{schoolRecordId}.");
            }
            var mapper = _mapper.CreateMapper();
            schoolRecord = mapper.Map(updateSchoolRecordRequest,schoolRecord);
            await UpdateAsyn(schoolRecord);
        }

        public async Task DeleteSchoolRecordById(int schoolRecordId, int studentId)
        {
            var schoolRecord = await FirstOrDefaultAsyn(sr => sr.Id == schoolRecordId && sr.Student.Id == studentId);
            if (schoolRecord == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy phiếu điểm(School Record) id:{schoolRecordId}.");
            }
            // not done yet . 
            await UpdateAsyn(schoolRecord);
        }
    }
}