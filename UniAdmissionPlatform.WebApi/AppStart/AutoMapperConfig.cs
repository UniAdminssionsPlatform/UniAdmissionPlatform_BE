﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.AutoMapperModules;
using UniAdmissionPlatform.BusinessTier.Commons;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    namespace UniAdmissionPlatform.WebApi.AppStart
    {
        public static class AutoMapperConfig
        {
            public static void ConfigureAutoMapperServices(this IServiceCollection services)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperResolver());
                    mc.ConfigTagMapperModule();
                    mc.ConfigEventMapperModule();
                    mc.ConfigEventTypeMapperModule();
                    mc.ConfigProvinceMapperModule();
                    mc.ConfigRoleMapperModule();
                    mc.ConfigSlotMapperModule();
                });
                var mapper = mappingConfig.CreateMapper();
                services.AddSingleton(mapper);
            }
        }
    }
}