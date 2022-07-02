using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class db_uapContext : DbContext
    {
        public db_uapContext()
        {
        }

        public db_uapContext(DbContextOptions<db_uapContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CasbinRule> CasbinRules { get; set; }
        public virtual DbSet<Certification> Certifications { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventCheck> EventChecks { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<GoalAdmission> GoalAdmissions { get; set; }
        public virtual DbSet<GoalAdmissionType> GoalAdmissionTypes { get; set; }
        public virtual DbSet<HighSchool> HighSchools { get; set; }
        public virtual DbSet<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<MajorDepartment> MajorDepartments { get; set; }
        public virtual DbSet<MajorGroup> MajorGroups { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsMajor> NewsMajors { get; set; }
        public virtual DbSet<NewsTag> NewsTags { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }
        public virtual DbSet<Participation> Participations { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SchoolRecord> SchoolRecords { get; set; }
        public virtual DbSet<SchoolYear> SchoolYears { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<Speaker> Speakers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentCertification> StudentCertifications { get; set; }
        public virtual DbSet<StudentRecordItem> StudentRecordItems { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectGroup> SubjectGroups { get; set; }
        public virtual DbSet<SubjectGroupMajor> SubjectGroupMajors { get; set; }
        public virtual DbSet<SubjectGroupSubject> SubjectGroupSubjects { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<UniversityEvent> UniversityEvents { get; set; }
        public virtual DbSet<UniversityNews> UniversityNews { get; set; }
        public virtual DbSet<UniversityProgram> UniversityPrograms { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=13.215.17.178,3306;Initial Catalog=db_uap;User ID=admin;Password=adminuap123;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.GenderId, "Account_Gender_Id_fk");

                entity.HasIndex(e => e.HighSchoolId, "Account_HighSchool_Id_fk");

                entity.HasIndex(e => e.OrganizationId, "Account_Organization_Id_fk");

                entity.HasIndex(e => e.RoleId, "Account_Role_Id_fk");

                entity.HasIndex(e => e.UniversityId, "Account_University_Id_fk");

                entity.HasIndex(e => e.WardId, "Account_Ward_Id_fk");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.EmailContact).HasColumnType("tinytext");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.IdCard).HasColumnType("tinytext");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.MiddleName).HasColumnType("tinytext");

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.PlaceOfBirth)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ProfileImageUrl).HasColumnType("tinytext");

                entity.Property(e => e.Religion).HasColumnType("tinytext");

                entity.Property(e => e.RoleId).HasMaxLength(20);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Account_Gender_Id_fk");

                entity.HasOne(d => d.HighSchool)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.HighSchoolId)
                    .HasConstraintName("Account_HighSchool_Id_fk");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Account)
                    .HasForeignKey<Account>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Account_User_Id_fk");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("Account_Organization_Id_fk");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("Account_Role_Id_fk");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("Account_University_Id_fk");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.WardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Account_Ward_Id_fk");
            });

            modelBuilder.Entity<CasbinRule>(entity =>
            {
                entity.ToTable("casbin_rule");

                entity.HasIndex(e => e.Ptype, "IX_casbin_rule_ptype");

                entity.HasIndex(e => e.V0, "IX_casbin_rule_v0");

                entity.HasIndex(e => e.V1, "IX_casbin_rule_v1");

                entity.HasIndex(e => e.V2, "IX_casbin_rule_v2");

                entity.HasIndex(e => e.V3, "IX_casbin_rule_v3");

                entity.HasIndex(e => e.V4, "IX_casbin_rule_v4");

                entity.HasIndex(e => e.V5, "IX_casbin_rule_v5");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ptype).HasColumnName("ptype");

                entity.Property(e => e.V0).HasColumnName("v0");

                entity.Property(e => e.V1).HasColumnName("v1");

                entity.Property(e => e.V2).HasColumnName("v2");

                entity.Property(e => e.V3).HasColumnName("v3");

                entity.Property(e => e.V4).HasColumnName("v4");

                entity.Property(e => e.V5).HasColumnName("v5");
            });

            modelBuilder.Entity<Certification>(entity =>
            {
                entity.ToTable("Certification");

                entity.HasIndex(e => e.DeletedAt, "ix_certification_deleted_at");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.HasIndex(e => e.ProvinceId, "District_Province_Id_fk");

                entity.Property(e => e.Name).HasColumnType("tinytext");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("District_Province_Id_fk");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.HasIndex(e => e.DistrictId, "Event_District_Id_fk");

                entity.HasIndex(e => e.EventTypeId, "Event_EventType_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_event_deleted_at");

                entity.Property(e => e.Address).HasColumnType("tinytext");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.MeetingUrl).HasColumnType("tinytext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("Event_District_Id_fk");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Event_EventType_Id_fk");
            });

            modelBuilder.Entity<EventCheck>(entity =>
            {
                entity.ToTable("EventCheck");

                entity.HasIndex(e => e.EventId, "EventCheck_Event_Id_fk");

                entity.HasIndex(e => e.SlotId, "EventCheck_Slot_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_event_check_deleted_at");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventChecks)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EventCheck_Event_Id_fk");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.EventChecks)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EventCheck_Slot_Id_fk");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.UniversityId })
                    .HasName("PRIMARY");

                entity.ToTable("Follow");

                entity.HasIndex(e => e.UniversityId, "Follow_University_Id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Follows)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Follow_Student_Id_fk");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Follows)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Follow_University_Id_fk");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.HasIndex(e => e.Id, "Gender_Id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_gender_deleted_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<GoalAdmission>(entity =>
            {
                entity.ToTable("GoalAdmission");

                entity.HasIndex(e => e.GoalAdmissionTypeId, "GoalAdmission_GoalAdmissionType_Id_fk");

                entity.HasIndex(e => e.SchoolYearId, "GoalAdmission_SchoolYear_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_goal_admission_deleted_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.TargetStudent).IsRequired();

                entity.HasOne(d => d.GoalAdmissionType)
                    .WithMany(p => p.GoalAdmissions)
                    .HasForeignKey(d => d.GoalAdmissionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoalAdmission_FK");

                entity.HasOne(d => d.SchoolYear)
                    .WithMany(p => p.GoalAdmissions)
                    .HasForeignKey(d => d.SchoolYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoalAdmission_SchoolYear_Id_fk");
            });

            modelBuilder.Entity<GoalAdmissionType>(entity =>
            {
                entity.ToTable("GoalAdmissionType");

                entity.HasIndex(e => e.Id, "GoalAdmissionType_Id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_goal_admission_type_deleted_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<HighSchool>(entity =>
            {
                entity.ToTable("HighSchool");

                entity.HasIndex(e => e.DistrictId, "HighSchool_District_Id_fk");

                entity.HasIndex(e => e.HighSchoolCode, "HighSchool_HighSchoolCode_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.HighSchoolManagerCode, "HighSchool_HighSchoolManagerCode_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_high_school_deleted_at");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.HighSchoolCode).HasMaxLength(20);

                entity.Property(e => e.HighSchoolManagerCode).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ProfileImageUrl).IsRequired();

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ThumbnailUrl).IsRequired();

                entity.Property(e => e.WebsiteUrl)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HighSchools)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HighSchool_District_Id_fk");
            });

            modelBuilder.Entity<HighSchoolEvent>(entity =>
            {
                entity.HasKey(e => new { e.HighSchoolId, e.EventId })
                    .HasName("PRIMARY");

                entity.ToTable("HighSchoolEvent");

                entity.HasIndex(e => e.EventId, "HighSchoolEvent_Event_Id_fk");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.HighSchoolEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HighSchoolEvent_Event_Id_fk");

                entity.HasOne(d => d.HighSchool)
                    .WithMany(p => p.HighSchoolEvents)
                    .HasForeignKey(d => d.HighSchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HighSchoolEvent_HighSchool_Id_fk");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("Major");

                entity.HasIndex(e => e.MajorGroupId, "Major_MajorGroup_Id_fk");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.MajorGroup)
                    .WithMany(p => p.Majors)
                    .HasForeignKey(d => d.MajorGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Major_MajorGroup_Id_fk");
            });

            modelBuilder.Entity<MajorDepartment>(entity =>
            {
                entity.ToTable("MajorDepartment");

                entity.HasIndex(e => e.MajorId, "MajorDepartment_Major_Id_fk");

                entity.HasIndex(e => e.MajorParentId, "MajorDepartment_Major_Id_fk_2");

                entity.HasIndex(e => e.UniversityId, "MajorDepartment_University_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_major_department_deleted_at");

                entity.Property(e => e.Name).HasColumnType("tinytext");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.MajorDepartmentMajors)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MajorDepartment_Major_Id_fk");

                entity.HasOne(d => d.MajorParent)
                    .WithMany(p => p.MajorDepartmentMajorParents)
                    .HasForeignKey(d => d.MajorParentId)
                    .HasConstraintName("MajorDepartment_Major_Id_fk_2");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.MajorDepartments)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MajorDepartment_University_Id_fk");
            });

            modelBuilder.Entity<MajorGroup>(entity =>
            {
                entity.ToTable("MajorGroup");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ThumbnailUrl).IsRequired();
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("Nationality");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasIndex(e => e.DeletedAt, "ix_news_deleted_at");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsPublish)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<NewsMajor>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.MajorId })
                    .HasName("PRIMARY");

                entity.ToTable("NewsMajor");

                entity.HasIndex(e => e.MajorId, "NewsMajor_Major_Id_fk");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.NewsMajors)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NewsMajor_Major_Id_fk");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsMajors)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NewsMajor_News_Id_fk");
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.TagId })
                    .HasName("PRIMARY");

                entity.ToTable("NewsTag");

                entity.HasIndex(e => e.TagId, "NewsTag_Tag_Id_fk");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NewsTag_News_Id_fk");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NewsTag_Tag_Id_fk");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");

                entity.HasIndex(e => e.OrganizationTypeId, "Organization_OrganizationType_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_organization_deleted_at");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.WebsiteUrl)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasColumnName("WebsiteURL");

                entity.HasOne(d => d.OrganizationType)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.OrganizationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Organization_OrganizationType_Id_fk");
            });

            modelBuilder.Entity<OrganizationEvent>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.EventId })
                    .HasName("PRIMARY");

                entity.ToTable("OrganizationEvent");

                entity.HasIndex(e => e.EventId, "OrganizationEvent_Event_Id_fk");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.OrganizationEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrganizationEvent_Event_Id_fk");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.OrganizationEvents)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrganizationEvent_Organization_Id_fk");
            });

            modelBuilder.Entity<OrganizationType>(entity =>
            {
                entity.ToTable("OrganizationType");

                entity.HasIndex(e => e.Id, "OrganizationType_Id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_organization_type_deleted_at");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Participation>(entity =>
            {
                entity.ToTable("Participation");

                entity.HasIndex(e => e.EventId, "Participation_Event_Id_fk");

                entity.HasIndex(e => e.StudentId, "Participation_Student_Id_fk");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Participation_Event_Id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Participation_Student_Id_fk");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.HasIndex(e => e.RegionId, "Province_Region_Id_fk");

                entity.Property(e => e.Name).HasColumnType("tinytext");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Province_Region_Id_fk");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.ToTable("Reason");

                entity.HasIndex(e => e.HighSchoolId, "RejectReason_HighSchool_Id_fk");

                entity.HasIndex(e => e.UniversityId, "RejectReason_University_Id_fk");

                entity.HasOne(d => d.HighSchool)
                    .WithMany(p => p.Reasons)
                    .HasForeignKey(d => d.HighSchoolId)
                    .HasConstraintName("RejectReason_HighSchool_Id_fk");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Reasons)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("RejectReason_University_Id_fk");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<SchoolRecord>(entity =>
            {
                entity.ToTable("SchoolRecord");

                entity.HasIndex(e => e.SchoolYearId, "SchoolRecord_SchoolYear_Id_fk");

                entity.HasIndex(e => e.StudentId, "SchoolRecord_Student_Id_fk");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.SchoolYear)
                    .WithMany(p => p.SchoolRecords)
                    .HasForeignKey(d => d.SchoolYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SchoolRecord_SchoolYear_Id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.SchoolRecords)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SchoolRecord_Student_Id_fk");
            });

            modelBuilder.Entity<SchoolYear>(entity =>
            {
                entity.ToTable("SchoolYear");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.HasIndex(e => e.HighSchoolId, "Slot_HighSchool_Id_fk");

                entity.HasOne(d => d.HighSchool)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.HighSchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Slot_HighSchool_Id_fk");
            });

            modelBuilder.Entity<Speaker>(entity =>
            {
                entity.ToTable("Speaker");

                entity.HasIndex(e => e.EventId, "Speaker_Event_Id_fk");

                entity.HasIndex(e => e.Organization, "Speaker_Organization_Id_fk");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Speakers)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Speaker_Event_Id_fk");

                entity.HasOne(d => d.OrganizationNavigation)
                    .WithMany(p => p.Speakers)
                    .HasForeignKey(d => d.Organization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Speaker_Organization_Id_fk");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.HasIndex(e => e.Id, "Student_Id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_student_deleted_at");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Student_Account_Id_fk");
            });

            modelBuilder.Entity<StudentCertification>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CertificationId })
                    .HasName("PRIMARY");

                entity.ToTable("StudentCertification");

                entity.HasIndex(e => e.CertificationId, "StudentCertification_Certification_Id_fk");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ImageUrl).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.Certification)
                    .WithMany(p => p.StudentCertifications)
                    .HasForeignKey(d => d.CertificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StudentCertification_Certification_Id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentCertifications)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StudentCertification_Student_Id_fk");
            });

            modelBuilder.Entity<StudentRecordItem>(entity =>
            {
                entity.ToTable("StudentRecordItem");

                entity.HasIndex(e => e.SchoolRecordId, "StudentRecordItem_SchoolRecord_Id_fk");

                entity.HasIndex(e => e.SubjectId, "StudentRecordItem_Subject_Id_fk");

                entity.HasOne(d => d.SchoolRecord)
                    .WithMany(p => p.StudentRecordItems)
                    .HasForeignKey(d => d.SchoolRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StudentRecordItem_SchoolRecord_Id_fk");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentRecordItems)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StudentRecordItem_Subject_Id_fk");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectGroup>(entity =>
            {
                entity.ToTable("SubjectGroup");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<SubjectGroupMajor>(entity =>
            {
                entity.HasKey(e => new { e.SubjectGroupId, e.MajorId })
                    .HasName("PRIMARY");

                entity.ToTable("SubjectGroupMajor");

                entity.HasIndex(e => e.MajorId, "SubjectGroupMajor_Major_Id_fk");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.SubjectGroupMajors)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubjectGroupMajor_Major_Id_fk");

                entity.HasOne(d => d.SubjectGroup)
                    .WithMany(p => p.SubjectGroupMajors)
                    .HasForeignKey(d => d.SubjectGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubjectGroupMajor_SubjectGroup_Id_fk");
            });

            modelBuilder.Entity<SubjectGroupSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectGroupId, e.SubjectId })
                    .HasName("PRIMARY");

                entity.ToTable("SubjectGroupSubject");

                entity.HasIndex(e => e.SubjectId, "SubjectGroupSubject_Subject_Id_fk");

                entity.HasOne(d => d.SubjectGroup)
                    .WithMany(p => p.SubjectGroupSubjects)
                    .HasForeignKey(d => d.SubjectGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubjectGroupSubject_SubjectGroup_Id_fk");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectGroupSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubjectGroupSubject_Subject_Id_fk");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.HasIndex(e => e.DeletedAt, "ix_tag_deleted_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("University");

                entity.HasIndex(e => e.DistrictId, "University_District_Id_fk");

                entity.HasIndex(e => e.UniversityCode, "University_UniversityCode_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.DeletedAt, "ix_university_deleted_at");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ProfileImageUrl).IsRequired();

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.ThumbnailUrl).IsRequired();

                entity.Property(e => e.UniversityCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.WebsiteUrl)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Universities)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("University_District_Id_fk");
            });

            modelBuilder.Entity<UniversityEvent>(entity =>
            {
                entity.HasKey(e => new { e.UniversityId, e.EventId })
                    .HasName("PRIMARY");

                entity.ToTable("UniversityEvent");

                entity.HasIndex(e => e.EventId, "UniversityEvent_Event_Id_fk");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.UniversityEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityEvent_Event_Id_fk");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.UniversityEvents)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityEvent_University_Id_fk");
            });

            modelBuilder.Entity<UniversityNews>(entity =>
            {
                entity.HasKey(e => new { e.UniversityId, e.NewsId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.NewsId, "UniversityNews_News_Id_fk");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.UniversityNews)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityNews_News_Id_fk");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.UniversityNews)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityNews_University_Id_fk");
            });

            modelBuilder.Entity<UniversityProgram>(entity =>
            {
                entity.ToTable("UniversityProgram");

                entity.HasIndex(e => e.MajorDepartmentId, "UniversityProgram_MajorDepartment_Id_fk");

                entity.HasIndex(e => e.SchoolYearId, "UniversityProgram_SchoolYear_Id_fk");

                entity.HasIndex(e => e.SubjectGroupId, "UniversityProgram_SubjectGroup_Id_fk");

                entity.HasIndex(e => e.DeletedAt, "ix_university_program_deleted_at");

                entity.Property(e => e.Name).HasColumnType("tinytext");

                entity.HasOne(d => d.MajorDepartment)
                    .WithMany(p => p.UniversityPrograms)
                    .HasForeignKey(d => d.MajorDepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityProgram_MajorDepartment_Id_fk");

                entity.HasOne(d => d.SchoolYear)
                    .WithMany(p => p.UniversityPrograms)
                    .HasForeignKey(d => d.SchoolYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UniversityProgram_SchoolYear_Id_fk");

                entity.HasOne(d => d.SubjectGroup)
                    .WithMany(p => p.UniversityPrograms)
                    .HasForeignKey(d => d.SubjectGroupId)
                    .HasConstraintName("UniversityProgram_SubjectGroup_Id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.DeletedAt, "ix_user_deleted_at");

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("Ward");

                entity.HasIndex(e => e.DistrictId, "Ward_District_Id_fk");

                entity.Property(e => e.Name).HasColumnType("tinytext");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Ward_District_Id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
