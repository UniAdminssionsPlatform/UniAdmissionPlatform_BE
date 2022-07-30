using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IFollowEventService
    {
        bool IsFollow(int studentId, int eventId);
        Task Follow(int studentId, int eventId, bool isFollow);
    }

    public partial class FollowEventService
    {
        public bool IsFollow(int studentId, int eventId)
        {
            return Get().Any(fe =>  (fe.Status == 0) &&
                fe.StudentId == studentId && fe.EventId == eventId);
        }

        public async Task Follow(int studentId, int eventId, bool isFollow)
        {
            if (!isFollow)
            {
                var followEvent = Get().FirstOrDefault(fe => fe.EventId == eventId && fe.StudentId == studentId);
                if (followEvent != null && followEvent.Status == (int?)FollowEventStatus.Followed)
                {
                    followEvent.Status = (int?)FollowEventStatus.Unfollowed;
                    await UpdateAsyn(followEvent);
                }
            }
            else
            {
                var followEvent = Get().FirstOrDefault(fe => fe.EventId == eventId && fe.StudentId == studentId);
                if (followEvent != null && followEvent.Status == (int?)FollowEventStatus.Unfollowed)
                {
                    followEvent.Status = (int?)FollowEventStatus.Followed;
                    await UpdateAsyn(followEvent);
                }

                if (followEvent == null)
                {
                    await CreateAsyn(new FollowEvent
                    {
                        StudentId = studentId,
                        EventId = eventId,
                        Status = (int?)FollowEventStatus.Unfollowed
                    });
                }
            }
        }
    }
}