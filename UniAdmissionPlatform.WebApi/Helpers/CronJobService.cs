using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.WebApi.Helpers
{
    public interface ICronJobService
    {
        Task CloseEventAutomatic();
    }

    public class CronJobService : ICronJobService
    {
        public Task CloseEventAutomatic()
        {
            db_uapContext _context = new db_uapContext();
            var eventType =_context!.EventTypes.FromSqlRaw("select * from EventType where Name = 'Offline In High School'").FirstOrDefault();
            return _context.Database
                .ExecuteSqlRawAsync("update Event set Status = 2, UpdatedAt = NOW() where EventTypeId != {0}" +
                                    " and EndTime is not null and EndTime <= NOW() and Status != 2;", eventType.Id);
        }
    }
}