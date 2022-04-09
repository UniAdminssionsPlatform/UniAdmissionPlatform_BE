﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class SwaggerConfig
    {
        public static void InitSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpiritAstro.WebApi", Version = "v1" });
                c.AddSecurityDefinition("x-token", new OpenApiSecurityScheme
                {
                    Name = "x-token",
                    In = ParameterLocation.Header
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                var provider = services.BuildServiceProvider();
                var service = provider.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var serviceApiVersionDescription in service.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(serviceApiVersionDescription.GroupName, CreateMetaInfoApiVersion(serviceApiVersionDescription));
                }
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-token" 
                            } 
                        },
                        System.Array.Empty<string>()
                    } 
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, SnakeCaseQueryParametersApiDescriptionProvider>());
        }
        
        private static OpenApiInfo CreateMetaInfoApiVersion(ApiVersionDescription apiDescription)
        {
            return new OpenApiInfo
            {
                Title = "Uni Admission Platform Open API",
                Version = apiDescription.ApiVersion.ToString(),
                Description = "This service provide api for UniAdmissionPlatform!",
            };
        }
    }
    
    
}