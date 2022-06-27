using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                c.RoutePrefix = string.Empty;
            });
        }

        public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                //c.DescribeAllEnumsAsStrings();
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

                c.OperationFilter<AuthorizeCheckOperationFilter>();

                var xmlFiles = System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory)
                    .Where(w => new FileInfo(w).Extension == ".xml");
                foreach (var file in xmlFiles)
                {
                    c.IncludeXmlComments(file);
                }

                c.AddSecurityDefinition("x-token", new OpenApiSecurityScheme
                {
                    Name = "x-token",
                    In = ParameterLocation.Header
                });
                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                c.EnableAnnotations();
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.TryAddEnumerable(ServiceDescriptor
                .Transient<IApiDescriptionProvider, SnakeCaseQueryParametersApiDescriptionProvider>());
        }

        public class AuthorizeCheckOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                bool hasAuth = (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                                    .Any()
                                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
                               && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

                var hiden = context.MethodInfo.CustomAttributes.FirstOrDefault(a =>
                    a.AttributeType == typeof(HiddenParamsAttribute));
                if (hiden != null)
                {
                    var parameters = ((string)hiden.ConstructorArguments.FirstOrDefault().Value).Split(",");
                    foreach (var parameter in parameters)
                    {
                        if (operation.Parameters.Any(a => a.Name.ToSnakeCase() == parameter.ToSnakeCase()))
                        {
                            operation.Parameters.Remove(operation.Parameters.FirstOrDefault(a =>
                                a.Name.ToSnakeCase() == parameter.ToSnakeCase()));
                        }
                    }
                }

                var hidenObject = context.MethodInfo.CustomAttributes.FirstOrDefault(a =>
                    a.AttributeType == typeof(HiddenObjectParamsAttribute));
                if (hidenObject != null)
                {
                    var parameters = ((string)hidenObject.ConstructorArguments.FirstOrDefault().Value).Split(",");
                    foreach (var parameter in parameters)
                    {
                        if (operation.Parameters.Any(a => a.Name.ToSnakeCase().Contains(parameter.ToSnakeCase())))
                        {
                            var openApiParameter = operation.Parameters.Where(a =>
                                a.Name.ToSnakeCase().Contains(parameter.ToSnakeCase())).ToList();
                            foreach (var apiParameter in openApiParameter)
                            {
                                operation.Parameters.Remove(apiParameter);
                            }
                        }
                    }
                }

                if (hasAuth)
                {
                    //operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    //operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                    //operation.Responses.Add("500", new OpenApiResponse { Description = "Forbidden" });
                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new OpenApiSecurityRequirement
                        {
                            [
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                }
                            ] = new string[] { }
                        }
                    };
                }
            }
        }

        //remove version required from route
        private class RemoveVersionParameterFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
                if (versionParameter == null) return;
                operation.Parameters.Remove(versionParameter);
            }
        }

        //auto map version from doc
        private class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = new OpenApiPaths();
                var hideApi = context.ApiDescriptions.Where(w =>
                    w.ActionDescriptor.EndpointMetadata.Any(a => a.GetType() == typeof(HiddenControllerAttribute)));
                foreach (var path in swaggerDoc.Paths)
                {
                    if (hideApi.Select(s => s.RelativePath.StartsWith("/") ? s.RelativePath : $"/{s.RelativePath}")
                        .Contains(path.Key))
                        continue;
                    paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
                }

                swaggerDoc.Paths = paths;
            }
        }
    }
}