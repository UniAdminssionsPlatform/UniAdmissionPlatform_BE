using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniAdmissionPlatform.BusinessTier.AutoMapperModules;
using UniAdmissionPlatform.BusinessTier.Commons;
using UniAdmissionPlatform.BusinessTier.Generations.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.UnitTest
{
    [TestClass]
    public class TestAssemblyInitialize
    {
        public static ServiceProvider ServiceProvider;
        
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            var services = new ServiceCollection();

            services.AddDbContext<db_uapContext>(
                opt => opt.UseMySQL("Server=13.215.17.178,3306;Initial Catalog=db_uap;User ID=admin;Password=adminuap123;Connection Timeout=30;Convert Zero Datetime=True;"));
            
            services.InitializerDI();
            
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
                mc.ConfigSubjectGroupMapperModule();
                mc.ConfigSlotMapperModule();
                mc.ConfigAccountMapperModule();
                mc.ConfigUserMapperModule();
                mc.ConfigHighSchoolMapperModule();
                mc.ConfigUniversityMapperModule();
                mc.ConfigEventCheckMapperModule();
                mc.ConfigSubjectMapperModule();
                mc.ConfigNewsMapperModule();
                mc.ConfigCertificationMapperModule();
                mc.ConfigDistrictMapperModule();
                mc.ConfigWardsMapperModule();
                mc.ConfigUniversityEventMapperModule();
                mc.ConfigStudentCertificationMapperModule();
                mc.ConfigParticipationMapperModule();
                mc.ConfigSubjectGroupMajorMapperModule();
                mc.ConfigMajorDepartmentModuleMapperModule();
                mc.ConfigNationalityMapperModule();
                mc.ConfigUniversityProgramModuleMapperModule();
                mc.ConfigSchoolRecordMapperModule();
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddSingleton<IAuthService, AuthService>();

            services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();
            
            

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}