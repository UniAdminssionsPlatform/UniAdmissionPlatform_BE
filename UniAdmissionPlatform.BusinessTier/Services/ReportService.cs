using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IReportService
    {
        string GetReportSetting(int eventId, string token);
        List<StudentReport> GetStudentReport(int eventId, int universityId);
    }
    public class ReportService : IReportService
    {
        private readonly db_uapContext _dbUapContext;

        public ReportService(db_uapContext dbUapContext)
        {
            _dbUapContext = dbUapContext;
        }

        public string GetReportSetting(int eventId, string token)
        {
            var reportSetting = _dbUapContext.ReportSettings.FirstOrDefault(rs => rs.Code == "EVENTSTUDENT");
            if (reportSetting == null)
            {
                return null;
            }

            var reportSettingContent = reportSetting.Content;
            reportSettingContent = reportSettingContent.Replace("event-id={0}", $"event-id={eventId}");
            reportSettingContent = reportSettingContent.Replace("token={1}", $"token={token}");
            return reportSettingContent;
        }

        public List<StudentReport> GetStudentReport(int eventId, int universityId)
        {
            var @event = _dbUapContext.Events.FirstOrDefault(e => e.Id == eventId && e.UniversityId == universityId);
            if (@event == null)
            {
                return null;
            }

            return _dbUapContext.StudentReports.FromSqlRaw(
                @"select E.Id                       as EventID,
       E.Name                     as EventName,
       A.FirstName                as FirstName,
       IFNULL(A.MiddleName, ' ')               as MiddleName,
       A.LastName                 as LastName,
       A.PhoneNumber              as PhoneNumber,
       IFNULL(A.EmailContact, '') as EmailContact,
       A.DateOfBirth              as DateOfBirth,
       HS.Id                      as HighSchoolId,
       HS.Name                    as HighSchoolname,
       W.Name                     as WardName,
       D.Name                     as DistrictName,
       P.Name                     as ProvinceName
from Event E
         join FollowEvent FE on E.Id = FE.EventId
         join Student S on FE.StudentId = S.Id
         join Account A on S.Id = A.Id
         join HighSchool HS on A.HighSchoolId = HS.Id
         join Ward W on A.WardId = W.Id
         join District D on W.DistrictId = D.Id
         join Province P on D.ProvinceId = P.Id
where EventId = {0}", eventId).ToList();
        }
    }
}