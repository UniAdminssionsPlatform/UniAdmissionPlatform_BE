using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUniversityService
    {
        Task<UniversityCodeViewModel> GetUniversityNameByCode(string highSchoolCode);
        
    }
    public partial class UniversityService
    {
        private readonly IConfigurationProvider _mapper;

        public UniversityService(IUnitOfWork unitOfWork, IUniversityRepository repository, IMapper mapper) : base(
            unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<UniversityCodeViewModel> GetUniversityNameByCode(string universityCode)
        {
            var university = await Get().ProjectTo<UniversityCodeViewModel>(_mapper).FirstOrDefaultAsync(u => u.UniversityCode == universityCode);
            if (university == null)
            {
                throw new ErrorResponse((int)(HttpStatusCode.NotFound),
                    "Không thể tìm thấy trường đại học nào ứng với mã đã cung cấp.");
            }

            return university;
        }
        
    }
}