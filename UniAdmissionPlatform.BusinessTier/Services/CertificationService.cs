using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Certification;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ICertificationService
    {
        Task<PageResult<CertificationBaseViewModel>> GetAllCertification(CertificationBaseViewModel filter, string sort, int page, int limit);
        Task<CertificationBaseViewModel> GetCertificationById(int certificationId);
        Task<int> CreateCertification(CreateCertificationRequest createCertificationRequest);
        Task UpdateCertification(int certificationId, UpdateCertificationRequest updateCertificationRequest);
        Task DeleteCertificationById(int certificationId);
    }
    public partial class CertificationService
    {
        private readonly IConfigurationProvider _mapper;

        public CertificationService(IUnitOfWork unitOfWork, ICertificationRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<CertificationBaseViewModel>> GetAllCertification(CertificationBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(c => c.DeletedAt == null)
                .ProjectTo<CertificationBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<CertificationBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<CertificationBaseViewModel> GetCertificationById(int certificationId)
        {
            var certification = await FirstOrDefaultAsyn(c => c.Id == certificationId && c.DeletedAt == null);
            if (certification == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chứng chỉ id = {certificationId}.");
            }
            
            return _mapper.CreateMapper().Map<CertificationBaseViewModel>(certification);
        }

        public async Task<int> CreateCertification(CreateCertificationRequest createCertificationRequest)
        {
            var certification = _mapper.CreateMapper().Map<Certification>(createCertificationRequest);
            await CreateAsyn(certification);
            certification.CreatedAt = DateTime.Now;
            certification.UpdatedAt = DateTime.Now;
            return certification.Id;
        }

        public async Task UpdateCertification(int certificationId, UpdateCertificationRequest updateCertificationRequest)
        {
            var certification = await FirstOrDefaultAsyn(c => c.Id == certificationId && c.DeletedAt == null);
            if (certification == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chứng chỉ id = {certificationId}.");
            }
            var mapper = _mapper.CreateMapper();
            certification = mapper.Map(updateCertificationRequest,certification);
            certification.UpdatedAt = DateTime.Now;
            await UpdateAsyn(certification);
        }

        public async Task DeleteCertificationById(int certificationId)
        {
            var certification = await FirstOrDefaultAsyn(c => c.Id == certificationId && c.DeletedAt == null);
            if (certification == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy chứng chỉ id = {certificationId}.");
            }
            certification.DeletedAt = DateTime.Now;
            await UpdateAsyn(certification);
        }
    }
}