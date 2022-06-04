using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Generations.Services;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IRecurringJobService
    {
        Task CloseEventAutomatic();
    }
    public class RecurringJobService : IRecurringJobService
    {
        private readonly IServiceProvider _serviceProvider;

        public RecurringJobService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task CloseEventAutomatic()
        {
            using var scope = _serviceProvider.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
            return eventService.CloseEvent();
        }
    }
}