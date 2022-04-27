using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IEventCheckService
    {
        Task ApproveEventToSlot(int highSchoolId, int eventCheckId);
        Task<EventBySlotBaseViewModel> GetEventBySlotId(int slotId);
    }

    public partial class EventCheckService
    {
        private readonly IConfigurationProvider _mapper;
        
        public EventCheckService(IUnitOfWork unitOfWork, IEventCheckRepository repository, IMapper mapper) : base(unitOfWork, 
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task ApproveEventToSlot(int highSchoolId, int eventCheckId)
        {
            var eventCheck = await Get(ec => ec.Id == eventCheckId)
                .Include(ec => ec.Slot)
                .Include(ec => ec.Event)
                .FirstOrDefaultAsync();
            
            if (eventCheck.Slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này không phải của trường bạn.");
            }
            
            if (eventCheck == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không tìm thấy cuộc đặt lịch này.");
            }

            if (eventCheck.Status != (int)EventCheckStatus.Pending)
            {
                if (eventCheck.Status == (int)EventCheckStatus.Approved)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này đã được chấp nhận.");
                }
                
                if (eventCheck.Status == (int)EventCheckStatus.Reject)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Cuộc đặt lịch này đã bị từ chối.");
                }
            }

            var uniEvent = eventCheck.Event;
            if (uniEvent.DeletedAt != null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy sự kiện trong cuộc đặt lịch");
            }

            if (uniEvent.Status != (int) EventStatus.OnGoing)
            {
                if (uniEvent.Status == (int) EventStatus.Cancel)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã bị hủy.");
                }
                
                if (uniEvent.Status == (int) EventStatus.Done)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Sự kiện này đã tổ chức rồi.");
                }
            }

            var slot = eventCheck.Slot;
            if (slot.Status != (int) SlotStatus.Open)
            {
                if (slot.Status == (int) SlotStatus.Close)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Buổi này đã bị đóng.");
                }
                
                if (slot.Status == (int) SlotStatus.Full)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Buổi này đã bị đầy lịch.");
                }
            }

            eventCheck.Status = (int)EventCheckStatus.Approved;
            await UpdateAsyn(eventCheck);
        }
        
        public async Task<EventBySlotBaseViewModel> GetEventBySlotId(int slotId)
        {
            var eventBySlot = await Get().Where(ec => ec.SlotId == slotId && ec.Status == (int)EventCheckStatus.Pending && ec.DeletedAt == null)
                .Include(ec => ec.Event)
                .ProjectTo<EventBySlotBaseViewModel>(_mapper).FirstOrDefaultAsync();
            if (eventBySlot == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy sự kiện nào theo slot với slot id ={eventBySlot}");
            }
            return eventBySlot;
        }
    }
}