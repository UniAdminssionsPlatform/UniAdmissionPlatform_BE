
/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UniAdmissionPlatform.DataTier.Models;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
namespace UniAdmissionPlatform.BusinessTier.Generations.DependencyInjection
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, db_uapContext>();
        
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        
            services.AddScoped<ICasbinRuleService, CasbinRuleService>();
            services.AddScoped<ICasbinRuleRepository, CasbinRuleRepository>();
        
            services.AddScoped<ICertificationService, CertificationService>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
        
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
        
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepository, EventRepository>();
        
            services.AddScoped<IEventCheckService, EventCheckService>();
            services.AddScoped<IEventCheckRepository, EventCheckRepository>();
        
            services.AddScoped<IEventTypeService, EventTypeService>();
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();
        
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<IFollowRepository, FollowRepository>();
        
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IGenderRepository, GenderRepository>();
        
            services.AddScoped<IGoalAdmissionService, GoalAdmissionService>();
            services.AddScoped<IGoalAdmissionRepository, GoalAdmissionRepository>();
        
            services.AddScoped<IGoalAdmissionTypeService, GoalAdmissionTypeService>();
            services.AddScoped<IGoalAdmissionTypeRepository, GoalAdmissionTypeRepository>();
        
            services.AddScoped<IGroupPointService, GroupPointService>();
            services.AddScoped<IGroupPointRepository, GroupPointRepository>();
        
            services.AddScoped<IHighSchoolService, HighSchoolService>();
            services.AddScoped<IHighSchoolRepository, HighSchoolRepository>();
        
            services.AddScoped<IHighSchoolEventService, HighSchoolEventService>();
            services.AddScoped<IHighSchoolEventRepository, HighSchoolEventRepository>();
        
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMajorRepository, MajorRepository>();
        
            services.AddScoped<IMajorDepartmentService, MajorDepartmentService>();
            services.AddScoped<IMajorDepartmentRepository, MajorDepartmentRepository>();
        
            services.AddScoped<IMajorGroupService, MajorGroupService>();
            services.AddScoped<IMajorGroupRepository, MajorGroupRepository>();
        
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<INationalityRepository, NationalityRepository>();
        
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsRepository, NewsRepository>();
        
            services.AddScoped<INewsMajorService, NewsMajorService>();
            services.AddScoped<INewsMajorRepository, NewsMajorRepository>();
        
            services.AddScoped<INewsTagService, NewsTagService>();
            services.AddScoped<INewsTagRepository, NewsTagRepository>();
        
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
        
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        
            services.AddScoped<IOrganizationEventService, OrganizationEventService>();
            services.AddScoped<IOrganizationEventRepository, OrganizationEventRepository>();
        
            services.AddScoped<IOrganizationTypeService, OrganizationTypeService>();
            services.AddScoped<IOrganizationTypeRepository, OrganizationTypeRepository>();
        
            services.AddScoped<IParticipationService, ParticipationService>();
            services.AddScoped<IParticipationRepository, ParticipationRepository>();
        
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
        
            services.AddScoped<IReasonService, ReasonService>();
            services.AddScoped<IReasonRepository, ReasonRepository>();
        
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionRepository, RegionRepository>();
        
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        
            services.AddScoped<ISchoolRecordService, SchoolRecordService>();
            services.AddScoped<ISchoolRecordRepository, SchoolRecordRepository>();
        
            services.AddScoped<ISchoolYearService, SchoolYearService>();
            services.AddScoped<ISchoolYearRepository, SchoolYearRepository>();
        
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<ISlotRepository, SlotRepository>();
        
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
        
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
        
            services.AddScoped<IStudentCertificationService, StudentCertificationService>();
            services.AddScoped<IStudentCertificationRepository, StudentCertificationRepository>();
        
            services.AddScoped<IStudentRecordItemService, StudentRecordItemService>();
            services.AddScoped<IStudentRecordItemRepository, StudentRecordItemRepository>();
        
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
        
            services.AddScoped<ISubjectGroupService, SubjectGroupService>();
            services.AddScoped<ISubjectGroupRepository, SubjectGroupRepository>();
        
            services.AddScoped<ISubjectGroupMajorService, SubjectGroupMajorService>();
            services.AddScoped<ISubjectGroupMajorRepository, SubjectGroupMajorRepository>();
        
            services.AddScoped<ISubjectGroupSubjectService, SubjectGroupSubjectService>();
            services.AddScoped<ISubjectGroupSubjectRepository, SubjectGroupSubjectRepository>();
        
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ITagRepository, TagRepository>();
        
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUniversityRepository, UniversityRepository>();
        
            services.AddScoped<IUniversityEventService, UniversityEventService>();
            services.AddScoped<IUniversityEventRepository, UniversityEventRepository>();
        
            services.AddScoped<IUniversityNewsService, UniversityNewsService>();
            services.AddScoped<IUniversityNewsRepository, UniversityNewsRepository>();
        
            services.AddScoped<IUniversityProgramService, UniversityProgramService>();
            services.AddScoped<IUniversityProgramRepository, UniversityProgramRepository>();
        
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        
            services.AddScoped<IWardService, WardService>();
            services.AddScoped<IWardRepository, WardRepository>();
        }
    }
}
