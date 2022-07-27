using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            return Get().Any(fe => fe.StudentId == studentId && fe.EventId == eventId);
        }

        public async Task Follow(int studentId, int eventId, bool isFollow)
        {
            if (!isFollow)
            {
                var followEvent = Get().FirstOrDefault(fe => fe.EventId == eventId && fe.StudentId == studentId);
                if (followEvent != null)
                {
                    await DeleteAsyn(followEvent);
                }
            }
            else
            {
                if (!IsFollow(studentId, eventId))
                {
                    await CreateAsyn(new FollowEvent
                    {
                        EventId = eventId,
                        StudentId = studentId
                    });
                }
            }
        }
    }
}