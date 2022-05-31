using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
        Task<PageResult<SchoolRecordBaseViewModel>> GetAllSchoolRecord(SchoolRecordBaseViewModel filter, string sort,
            int page, int limit);

        Task<int> CreateSchoolRecord(int studentId, CreateSchoolRecordRequest createSchoolRecordRequest);
        Task UpdateSchoolRecord(int schoolRecordId, int studentId, UpdateSchoolRecordRequest updateSchoolRecordRequest);
        Task DeleteSchoolRecordById(int schoolRecordId, int studentId);
        byte[] GetImportSchoolRecordExcel();
        Task ImportSchoolRecord(int studentId, int schoolYearId, IFormFile file);
    }

    public partial class SchoolRecordService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly ISubjectRepository _subjectRepository;

        public SchoolRecordService(IUnitOfWork unitOfWork, ISchoolRecordRepository repository, IMapper mapper,
            ISubjectRepository subjectRepository) : base(unitOfWork,
            repository)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<SchoolRecordBaseViewModel>> GetAllSchoolRecord(SchoolRecordBaseViewModel filter,
            string sort, int page, int limit)
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

        public async Task UpdateSchoolRecord(int schoolRecordId, int studentId,
            UpdateSchoolRecordRequest updateSchoolRecordRequest)
        {
            var schoolRecord = await FirstOrDefaultAsyn(sr => sr.Id == schoolRecordId && sr.Student.Id == studentId);
            if (schoolRecord == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    $"Không tìm thấy phiếu điểm(School Record) id:{schoolRecordId}.");
            }

            var mapper = _mapper.CreateMapper();
            schoolRecord = mapper.Map(updateSchoolRecordRequest, schoolRecord);
            await UpdateAsyn(schoolRecord);
        }

        public async Task DeleteSchoolRecordById(int schoolRecordId, int studentId)
        {
            var schoolRecord = await FirstOrDefaultAsyn(sr => sr.Id == schoolRecordId && sr.Student.Id == studentId);
            if (schoolRecord == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    $"Không tìm thấy phiếu điểm(School Record) id:{schoolRecordId}.");
            }

            // not done yet . 
            await UpdateAsyn(schoolRecord);
        }

        public byte[] GetImportSchoolRecordExcel()
        {
            using var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Bảng điểm";

            var worksheet = package.Workbook.Worksheets.Add("Điểm");
            var cells = worksheet.Cells;
            cells[1, 1].Value = "Môn học";
            cells[1, 2].Value = "Điểm trung bình môn học";

            var subjects = _subjectRepository.Get().ToList();
            

            for (var i = 0; i < subjects.Count; i++)
            {
                cells[i + 2, 1].Value = subjects[i].Name;
            }
            
            cells[2, 2, 62, 2].Style.Numberformat.Format = "0.0";
            
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();


            return package.GetAsByteArray();
        }
        

        public async Task ImportSchoolRecord(int studentId, int schoolYearId, IFormFile file)
        {
            var schoolRecord = await FirstOrDefaultAsyn(sr => sr.StudentId == studentId && sr.SchoolYearId == schoolYearId);
            if (schoolRecord != null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Đã tồn tại phiếu điểm này.");
            }
            

            await using var stream = file.OpenReadStream();
            using var package = new ExcelPackage(stream);

            var points = ReadPointFromExcel(package.Workbook.Worksheets["Điểm"]);
            var newSchoolRecord = new SchoolRecord
            {
                StudentRecordItems = new List<StudentRecordItem>(),
                StudentId = studentId,
                SchoolYearId = schoolYearId,
                Name = "default"
            };

            var stringBuilder = new StringBuilder();
            foreach (var (subjectName, point) in points)
            {
                if (point is < 0 or > 10)
                {
                    stringBuilder.Append($"Điểm không hợp lệ ({point}) cho môn {subjectName}.\n");
                };
                
                var subject = await _subjectRepository.FirstOrDefaultAsync(s => s.Name == subjectName);

                if (subject == null)
                {
                    stringBuilder.Append($"Tên môn không hợp lệ ({subjectName})");
                }

                else
                {
                    newSchoolRecord.StudentRecordItems.Add(new StudentRecordItem
                    {
                        SubjectId = subject.Id,
                        Score = (float?)point
                    });
                }
            }

            if (stringBuilder.Length  != 0)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, stringBuilder.ToString());
            }
            await CreateAsyn(newSchoolRecord);
        }

        private Dictionary<string, double> ReadPointFromExcel(ExcelWorksheet worksheet)
        {
            var subjectCount = worksheet.Dimension?.Rows - 1 ?? 0;
            var result = new Dictionary<string, double>();
            for (var i = 0; i < subjectCount; i++)
            {
                var subject = (string)worksheet.Cells[2 + i, 1].Value;
                var point = worksheet.Cells[2 + i, 2].Value;
                if (point != null)
                {
                    result[subject] = (double)point;
                }
            }

            return result;
        }
    }
}