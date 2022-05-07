using System;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniAdmissionPlatform.BusinessTier.Generations.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.WebApi.AppStart;
using UniAdmissionPlatform.WebApi.AppStart.UniAdmissionPlatform.WebApi.AppStart;
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
            //config production
            services.AddControllersWithViews();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });
            //end
            services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.ConfigureSwaggerServices();
            
            services.AddSingleton<ICasbinService, CasbinService>();

            services.ConfigureJsonFormatServices();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(
                    new MySqlStorage(
                        Configuration.GetConnectionString("HangFire"),
                        new MySqlStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            PrepareSchemaIfNecessary = true,
                            DashboardJobListLimit = 25000,
                            TransactionTimeout = TimeSpan.FromMinutes(1),
                            TablesPrefix = "Hangfire",
                        }
                    )
                ));
            
            // services.AddHangfireServer(options => options.WorkerCount = 1);

            services.InitFirebase();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.ConfigureVersioningServices();

            services.ConfigureFilterServices();

            services.InitializerDI();

            services.ConfigureAutoMapperServices();

            services.AddDbContext<db_uapContext>(
                opt => opt.UseMySQL(Configuration["ConnectionStrings:DbContext"]));

            services.AddSingleton<IAuthService, AuthService>();

            services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();

            services.ConfigureAutoMapperServices();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            //end
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            app.UseHangfireDashboard();
            
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
            app.UseRouting();
            
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.ConfigureSwagger(provider);
            
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    IgnoreAntiforgeryToken = true
                });
            });
        }
    }
}