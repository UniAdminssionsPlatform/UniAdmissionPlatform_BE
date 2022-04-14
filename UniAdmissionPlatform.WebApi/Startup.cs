using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UniAdmissionPlatform.BusinessTier.Generations.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.AppStart;
using UniAdmissionPlatform.WebApi.Middlewares;

namespace UniAdmissionPlatform.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            
            services.InitSwagger();
            
            services.AddSingleton<ICasbinService, CasbinService>();
            
            services.ConfigureJsonFormatServices();
            
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.ConfigureVersioningServices();

            services.ConfigureFilterServices();

            services.InitializerDI();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IApiVersionDescriptionProvider provider)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniAdmissionPlatform.WebApi v1"));
            // }

            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            
            app.UseSwaggerUI(c =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("UniAdmissionPlatform.WebApi.Resources.Swagger.index.html");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniAdmissionPlatform.WebApi v1");
            });

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}