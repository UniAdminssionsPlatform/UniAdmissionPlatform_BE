using AutoMapper;
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
                    mc.ConfigMajorMapperModule();
                    mc.ConfigEventTypeMapperModule();
                    mc.ConfigMajorGroupMapperModule();
                    mc.ConfigProvinceMapperModule();
                    mc.ConfigRoleMapperModule();
                    mc.ConfigSlotMapperModule();
                    mc.ConfigAccountMapperModule();
                    mc.ConfigUserMapperModule();
                    mc.ConfigHighSchoolMapperModule();
                    mc.ConfigUniversityMapperModule();
                    mc.ConfigEventCheckMapperModule();
                    mc.ConfigSubjectMapperModule();
                    mc.ConfigNewsMapperModule();
                    mc.ConfigCertificationMapperModule();

                });
                var mapper = mappingConfig.CreateMapper();
                services.AddSingleton(mapper);
            }
        }
    }
}