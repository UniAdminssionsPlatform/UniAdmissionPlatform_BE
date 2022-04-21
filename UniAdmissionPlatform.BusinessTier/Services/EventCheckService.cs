using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Responses;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IEventCheckService
    {
        Task ApproveEventToSlot(int highSchoolId, int eventCheckId);
    }

    public partial class EventCheckService
    {
        public async Task ApproveEventToSlot(int highSchoolId, int eventCheckId)
        {
            var eventCheck = await Get(ec => ec.Id == eventCheckId)
                .Include(ec => ec.Slot)
                .Include(ec => ec.Event)
                .FirstOrDefaultAsync();
            
            if (eventCheck.Slot.HighSchoolId != highSchoolId)
            {
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cuộc đặt lịch này không phải của trường bạn.");
            }
            
            if (eventCheck == null)
            {
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Không tìm thấy cuộc đặt lịch này.");
            }

            if (eventCheck.Status != (int)EventCheckStatus.Pending)
            {
                if (eventCheck.Status == (int)EventCheckStatus.Approved)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cuộc đặt lịch này đã được chấp nhận.");
                }
                
                if (eventCheck.Status == (int)EventCheckStatus.Reject)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cuộc đặt lịch này đã bị từ chối.");
                }
            }

            var uniEvent = eventCheck.Event;
            if (uniEvent.DeletedAt != null)
            {
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Không thể tìm thấy sự kiện trong cuộc đặt lịch");
            }

            if (uniEvent.Status != (int) EventStatus.OnGoing)
            {
                if (uniEvent.Status == (int) EventStatus.Cancel)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Sự kiện này đã bị hủy.");
                }
                
                if (uniEvent.Status == (int) EventStatus.Done)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Sự kiện này đã tổ chức rồi.");
                }
            }

            var slot = eventCheck.Slot;
            if (slot.Status != (int) SlotStatus.Open)
            {
                if (slot.Status == (int) SlotStatus.Close)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Buổi này đã bị đóng.");
                }
                
                if (slot.Status == (int) SlotStatus.Full)
                {
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Buổi này đã bị đầy lịch.");
                }
            }

            eventCheck.Status = (int)EventCheckStatus.Approved;
            await UpdateAsyn(eventCheck);
        }
    }
}