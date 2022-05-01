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
using UniAdmissionPlatform.BusinessTier.Requests.StudentCertificaiton;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IStudentCertificationService
    {
        Task<PageResult<StudentCertificationBaseViewModel>> GetAllStudentCertification(StudentCertificationBaseViewModel filter, string sort, int page, int limit);
        Task<int> CreateStudentCertification(int studentId, CreateStudentCertificationRequest createStudentCertificationRequest);
        Task UpdateStudentCertification(int studentId, int certificationId, UpdateStudentCertificationRequest updateStudentCertificationRequest);
        Task DeleteStudentCertificationById(int studentId, int certificationId);
    }
    public partial class StudentCertificationService
    {
        private readonly IConfigurationProvider _mapper;

        public StudentCertificationService(IUnitOfWork unitOfWork, IStudentCertificationRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<StudentCertificationBaseViewModel>> GetAllStudentCertification(StudentCertificationBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<StudentCertificationBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<StudentCertificationBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        

        public async Task<int> CreateStudentCertification(int studentId, CreateStudentCertificationRequest createStudentCertificationRequest)
        {
            var stuCertification = _mapper.CreateMapper().Map<StudentCertification>(createStudentCertificationRequest);
            stuCertification.StudentId = studentId ;
            
            await CreateAsyn(stuCertification);
            return stuCertification.CertificationId;
        }

        public async Task UpdateStudentCertification(int studentId, int certificationId, UpdateStudentCertificationRequest updateStudentCertificationRequest)
        {
            var stuCertification = await FirstOrDefaultAsyn(s => s.StudentId == studentId && s.CertificationId == certificationId);
            if (stuCertification == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chứng chỉ id:{certificationId} của học sinh id:{studentId}.");
            }
            var mapper = _mapper.CreateMapper();
            stuCertification = mapper.Map(updateStudentCertificationRequest,stuCertification);
            await UpdateAsyn(stuCertification);
        }

        public async Task DeleteStudentCertificationById(int studentId, int certificationId)
        {
            var stuCertification = await FirstOrDefaultAsyn(s => s.CertificationId == certificationId && s.StudentId == studentId);
            if (stuCertification == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chứng chỉ id:{certificationId} của học sinh id:{studentId}.");
            }
            await UpdateAsyn(stuCertification);
        }
    }
}