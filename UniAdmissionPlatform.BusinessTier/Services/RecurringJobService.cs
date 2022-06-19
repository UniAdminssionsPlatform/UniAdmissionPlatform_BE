using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IRecurringJobService
    {
        Task CloseEventAutomatic();
        Task CloseSlotAutomatic();
    }
    public class RecurringJobService : IRecurringJobService
    {
        private readonly db_uapContext _dbUapContext;

        public RecurringJobService(db_uapContext dbUapContext)
        {
            _dbUapContext = dbUapContext;
        }

        public async Task CloseEventAutomatic()
        {
            var events = await _dbUapContext.Events.Where(e => e.EndTime < DateTime.Now && e.Status != (int)EventStatus.Done && e.DeletedAt == null).ToListAsync();
            events.ForEach(e => e.Status = (int)EventStatus.Done);
            await _dbUapContext.SaveChangesAsync();
        }

        public async Task CloseSlotAutomatic()
        {
            var slots = await _dbUapContext.Slots.Where(s => s.EndDate < DateTime.Now && s.Status != (int)SlotStatus.Close)
                .ToListAsync();
            slots.ForEach(s => s.Status = (int) SlotStatus.Close);
            await _dbUapContext.SaveChangesAsync();
        }
    }
}