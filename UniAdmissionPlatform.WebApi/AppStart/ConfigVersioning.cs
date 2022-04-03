using Microsoft.Extensions.DependencyInjection;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class ConfigVersioning
    {
        public static void ConfigureVersioningServices(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(opt =>
                {
                    opt.GroupNameFormat = "'v'VVV";
                    opt.SubstituteApiVersionInUrl = true;
                }
            );
        }
    }
}