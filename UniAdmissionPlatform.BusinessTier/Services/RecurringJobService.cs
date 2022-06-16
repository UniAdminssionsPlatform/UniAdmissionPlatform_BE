using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IRecurringJobService
    {
        Task CloseEventAutomatic();
    }
    public class RecurringJobService : IRecurringJobService
    {
        private readonly db_uapContext _dbUapContext;

        public RecurringJobService(db_uapContext dbUapContext)
        {
            _dbUapContext = dbUapContext;
        }

        public Task CloseEventAutomatic()
        {
            var events = _dbUapContext.Events.Where(e => e.EndTime != null && e.EndTime < DateTime.Now && e.Status != (int)EventStatus.Done && e.DeletedAt == null).ToList();
            events.ForEach(e => e.Status = (int)EventStatus.Done);
            return _dbUapContext.SaveChangesAsync();
        }
    }
}