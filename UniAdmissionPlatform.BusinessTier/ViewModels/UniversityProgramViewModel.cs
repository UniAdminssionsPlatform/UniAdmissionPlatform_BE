using System.Collections.Generic;
using System.Linq;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class UniversityProgramBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? MajorDepartmentId { get; set; }
        [String]
        public string Description { get; set; }
        public int? SchoolYearDescription { get; set; }
        public int? SchoolYearId { get; set; }
    }

    public class UniversityProgramWithMajorDepartmentAndSchoolYearModel : UniversityProgramBaseViewModel
    {
        public MajorDepartmentWithMajorAndUniversity MajorDepartment { get; set; }
        public SchoolYearBaseViewModel SchoolYear { get; set; }
    }

    public class MajorWrapper
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class SubjectGroupWrapper
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UniversityProgramAdmissionWrapper
    {
        public int? ParentProgramId { get; set; }
        public string ParentProgramName { get; set; }
        public string ParentProgramCode { get; set; }
        public List<MajorWrapper> Majors { get; set; }
        public List<SubjectGroupWrapper> SubjectGroups { get; set; }
        public int? SumOfQuantity { get; set; }

        public UniversityProgramAdmissionWrapper()
        {
            Majors = new List<MajorWrapper>();
            SubjectGroups = new List<SubjectGroupWrapper>();
        }
    }  

    public class ListUniversityProgramAdmission
    {
        public List<UniversityProgramAdmissionWrapper> UniversityProgramAdmissions { get; set; }

        public ListUniversityProgramAdmission(List<UniversityProgramAdmission> universityProgramAdmissions)
        {
            var dictionary = new Dictionary<int, UniversityProgramAdmissionWrapper>();
            var defaultUniversityProgramAdmissionWrapper = new UniversityProgramAdmissionWrapper();
            foreach (var universityProgramAdmission in universityProgramAdmissions)
            {
                if (universityProgramAdmission.ParentId == null)
                {
                    if (!defaultUniversityProgramAdmissionWrapper.Majors.Any(m => m.Id != universityProgramAdmission.MajorId))
                    {
                        defaultUniversityProgramAdmissionWrapper.Majors.Add(new MajorWrapper
                        {
                            Id = universityProgramAdmission.MajorId,
                            Code = universityProgramAdmission.MajorCode,
                            Name = universityProgramAdmission.MajorName
                        });
                    }

                    if (!defaultUniversityProgramAdmissionWrapper.SubjectGroups.Any(sg => sg.Id != universityProgramAdmission.SubjectGroupId))
                    {
                        defaultUniversityProgramAdmissionWrapper.SubjectGroups.Add(new SubjectGroupWrapper
                        {
                            Id = universityProgramAdmission.SubjectGroupId,
                            Name = universityProgramAdmission.SubjectGroupName
                        });
                    }

                    defaultUniversityProgramAdmissionWrapper.SumOfQuantity += universityProgramAdmission.Quantity ?? 0;
                }
                else
                {
                    if (dictionary.ContainsKey(universityProgramAdmission.ParentId.Value))
                    {
                        if (!dictionary[universityProgramAdmission.ParentId.Value].Majors.Any(m => m.Id == universityProgramAdmission.MajorId))
                        {
                            dictionary[universityProgramAdmission.ParentId.Value].Majors.Add(new MajorWrapper
                            {
                                Id = universityProgramAdmission.MajorId,
                                Code = universityProgramAdmission.MajorCode,
                                Name = universityProgramAdmission.MajorName
                            });
                        }

                        if (!dictionary[universityProgramAdmission.ParentId.Value].SubjectGroups.Any(sg => sg.Id == universityProgramAdmission.SubjectGroupId))
                        {
                            dictionary[universityProgramAdmission.ParentId.Value].SubjectGroups.Add(new SubjectGroupWrapper
                            {
                                Id = universityProgramAdmission.SubjectGroupId,
                                Name = universityProgramAdmission.SubjectGroupName
                            });
                        }

                        dictionary[universityProgramAdmission.ParentId.Value].SumOfQuantity +=
                            universityProgramAdmission.Quantity ?? 0;
                    }
                    else
                    {
                        var universityProgramAdmissionWrapper = new UniversityProgramAdmissionWrapper
                        {
                            ParentProgramId = universityProgramAdmission.ParentId.Value,
                            ParentProgramCode = universityProgramAdmission.ParentCode,
                            ParentProgramName = universityProgramAdmission.ParentName,
                        };
                        
                        universityProgramAdmissionWrapper.Majors.Add(new MajorWrapper
                        {
                            Id = universityProgramAdmission.MajorId,
                            Code = universityProgramAdmission.MajorCode,
                            Name = universityProgramAdmission.MajorName
                        });
                        
                        universityProgramAdmissionWrapper.SubjectGroups.Add(new SubjectGroupWrapper
                        {
                            Id = universityProgramAdmission.SubjectGroupId,
                            Name = universityProgramAdmission.SubjectGroupName
                        });

                        universityProgramAdmissionWrapper.SumOfQuantity = universityProgramAdmission.Quantity ?? 0;
                        dictionary.Add(universityProgramAdmission.ParentId.Value, universityProgramAdmissionWrapper);
                    }
                }
            }

            UniversityProgramAdmissions = dictionary.Values.ToList();
            foreach (var universityProgramAdmissionWrapper in UniversityProgramAdmissions)
            {
                if (universityProgramAdmissionWrapper.SumOfQuantity == 0)
                {
                    universityProgramAdmissionWrapper.SumOfQuantity = null;
                }
            }
        }
    }
    
    public class UniversityProgramAdmission
    {
        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public int MajorId { get; set; }
        public string MajorCode { get; set; }
        public string MajorName { get; set; }
        public int SubjectGroupId { get; set; }
        public string SubjectGroupName { get; set; }
        public int? Quantity { get; set; }
    }
}