using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Participation;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IParticipationService
    {
        Task<PageResult<ParticipationBaseViewModel>> GetParticipations(ParticipationBaseViewModel filter,
            string sort, int page, int limit);
        Task<int> CreateParticipation(int studentId, CreateParticipationRequestForStudent createParticipationRequestForStudent);
        Task UpdateParticipation(int id, UpdateParticipationRequestForStudent updateParticipationRequestForStudent, int studentId = 0);
        Task DeleteParticipation(int id, int studentId = 0);
        Task<ParticipationBaseViewModel> GetById(int id);
    }
    public partial class ParticipationService
    {
        private readonly IConfigurationProvider _mapper;
        
        public ParticipationService(IUnitOfWork unitOfWork, IParticipationRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 200;
        private const int DefaultPaging = 10;
        
        public async Task<PageResult<ParticipationBaseViewModel>> GetParticipations(ParticipationBaseViewModel filter,
            string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .ProjectTo<ParticipationBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<ParticipationBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<int> CreateParticipation(int studentId, CreateParticipationRequestForStudent createParticipationRequestForStudent)
        {
            var participation = _mapper.CreateMapper().Map<Participation>(createParticipationRequestForStudent);
            participation.StudentId = studentId;

            await CreateAsyn(participation);
            return participation.Id;
        }

        public async Task UpdateParticipation(int id, UpdateParticipationRequestForStudent updateParticipationRequestForStudent, int studentId = 0)
        {
            var participation = await FirstOrDefaultAsyn(p => p.Id == id);
            
            if (participation == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không thể tìm thấy sự tham gia này.");
            }

            if (studentId != 0 && participation.StudentId != studentId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự tham gia này không phải của bạn.");
            }

            _mapper.CreateMapper().Map(updateParticipationRequestForStudent, participation);

            await UpdateAsyn(participation);
        }

        public async Task DeleteParticipation(int id, int studentId = 0)
        {
            var participation = await FirstOrDefaultAsyn(p => p.Id == id);
            
            if (participation == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không thể tìm thấy sự tham gia này.");
            }

            if (studentId != 0 && participation.StudentId != studentId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự tham gia này không phải của bạn.");
            }

            await DeleteAsyn(participation);
        }

        public async Task<ParticipationBaseViewModel> GetById(int id)
        {
            var participation = await FirstOrDefaultAsyn(p => p.Id == id);
            
            if (participation == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy sự tham gia này.");
            }

            return _mapper.CreateMapper().Map<ParticipationBaseViewModel>(participation);
        }
    }
}