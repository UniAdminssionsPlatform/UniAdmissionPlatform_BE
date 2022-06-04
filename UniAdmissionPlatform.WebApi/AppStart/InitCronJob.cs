using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class InitCronJob
    {
        public static void InitCronJobVoid(this IServiceCollection services)
        {
            RecurringJob.AddOrUpdate<IRecurringJobService>("close-event", rjs => rjs.CloseEventAutomatic(), "0 * * ? * *	");
        }
    }
}