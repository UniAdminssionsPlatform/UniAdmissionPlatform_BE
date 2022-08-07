using System;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.University;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityService
    {
        Task<UniversityCodeViewModel> GetUniversityNameByCode(string universityCode);
        Task<PageResult<UniversityBaseViewModel>> GetAllUniversities(UniversityBaseViewModel filter, string sort, int page, int limit, int? userId = null);
        Task<UniversityBaseViewModel> GetUniversityByID(int Id, int? userId = null);

        Task<int> CreateUniversity(CreateUniversityRequest createUniversityRequest);
        Task UpdateUniversityProfile(int universityId, UpdateUniversityProfileRequest updateUniversityProfileRequest);
    }
    public partial class UniversityService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IFollowRepository _followRepository;

        public UniversityService(IUnitOfWork unitOfWork, IUniversityRepository repository, IMapper mapper, IFollowRepository followRepository) : base(
            unitOfWork,
            repository)
        {
            _followRepository = followRepository;
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<UniversityCodeViewModel> GetUniversityNameByCode(string universityCode)
        {
            var university = await Get()
                .Where(u => u.DeletedAt == null)
                .ProjectTo<UniversityCodeViewModel>(_mapper).FirstOrDefaultAsync(u => u.UniversityCode == universityCode);
            if (university == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound,
                    "Không thể tìm thấy trường đại học nào ứng với mã đã cung cấp.");
            }

            return university;
        }
        
         
        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<UniversityBaseViewModel>> GetAllUniversities(UniversityBaseViewModel filter, string sort, int page, int limit, int? userId = null)
        {
            var (total, queryable) = Get
                    (s => s.Status == (int)UniversityStatus.Active && s.DeletedAt == null)
                .ProjectTo<UniversityBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            var universityBaseViewModels = await queryable.ToListAsync();

            if (userId != null)
            {
                var follows = _followRepository.Get().Where(f => f.StudentId == userId && universityBaseViewModels.Select(u => u.Id).Contains(f.UniversityId))
                    .ToDictionary(f => f.UniversityId, f => f);
                foreach (var universityBaseViewModel in universityBaseViewModels)
                {
                    universityBaseViewModel.IsFollow =
                        follows.ContainsKey(universityBaseViewModel.Id!.Value)
                        && follows[universityBaseViewModel.Id!.Value].Status == (int?)FollowUniversityStatus.Followed;
                }
            }
            
            return new PageResult<UniversityBaseViewModel>
            {
                List = universityBaseViewModels,
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<UniversityBaseViewModel> GetUniversityByID(int universityId, int? userId = null)
        {
            var universityById = await Get().Where(u => u.Id == universityId && u.Status == (int)UniversityStatus.Active && u.DeletedAt == null)
                .ProjectTo<UniversityBaseViewModel>(_mapper).FirstOrDefaultAsync();

            if (userId != null)
            {
                var follow = _followRepository.Get()
                    .FirstOrDefault(f => f.StudentId == userId && f.UniversityId == universityId);
                universityById.IsFollow = follow != null && follow.Status == (int?)FollowUniversityStatus.Followed;
            }

            if (universityById == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy trường đại học nào có id = {universityId}");
            }

            return universityById;
        }

        public async Task<int> CreateUniversity(CreateUniversityRequest createUniversityRequest)
        {
            var mapper = _mapper.CreateMapper();
            var university = mapper.Map<University>(createUniversityRequest);

            if (await Get().AnyAsync(u => u.UniversityCode == createUniversityRequest.UniversityCode && u.DeletedAt == null))
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    "Mã của trường đại học đã tồn tại.");
            }

            await CreateAsyn(university);
            return university.Id;
        }
        
        public async Task UpdateUniversityProfile(int universityId, UpdateUniversityProfileRequest updateUniversityProfileRequest)
        {
            var universityAccount = await Get().Where(au => au.Id == universityId && au.DeletedAt == null).FirstOrDefaultAsync();
            if (universityAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy university với id = {universityId}");
            }
            
            var mapper = _mapper.CreateMapper();
            universityAccount = mapper.Map(updateUniversityProfileRequest, universityAccount);
            universityAccount.UpdatedAt = DateTime.Now;
            
            await UpdateAsyn(universityAccount);
        }
    }
}