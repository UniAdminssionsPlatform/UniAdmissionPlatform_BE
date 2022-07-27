/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
namespace UniAdmissionPlatform.BusinessTier.Generations.Repositories
{
    public partial interface IReportSettingRepository :IBaseRepository<ReportSetting>
    {
    }
    public partial class ReportSettingRepository :BaseRepository<ReportSetting>, IReportSettingRepository
    {
         public ReportSettingRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

