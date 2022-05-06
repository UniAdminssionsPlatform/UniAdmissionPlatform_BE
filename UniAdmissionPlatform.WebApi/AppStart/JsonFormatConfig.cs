using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class JsonFormatConfig
    {
        public static void ConfigureJsonFormatServices(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
        }
    }
}
