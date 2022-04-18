using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface ISlotService
    {
        Task<int> CreateSlot(int highSchoolId, CreateSlotRequest createSlotRequest);
    }
    
    public partial class SlotService
    {
        private readonly IConfigurationProvider _mapper;

        public SlotService(IUnitOfWork unitOfWork, ISlotRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<int> CreateSlot(int highSchoolId, CreateSlotRequest createSlotRequest)
        {
            var mapper = _mapper.CreateMapper();
            var slot = mapper.Map<Slot>(createSlotRequest);

            if (slot.EndTime != null && slot.StartTime <= slot.EndTime)
            {
                throw new ErrorResponse((int)HttpStatusCode.BadRequest,
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
            }
            
            slot.HighSchoolId = highSchoolId;
            slot.Status = (int)SlotStatus.Open;

            await CreateAsyn(slot);
            return slot.Id;
        }
    }
}