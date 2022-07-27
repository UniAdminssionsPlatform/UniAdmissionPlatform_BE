/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    
    public partial interface IReportSettingService:IBaseService<ReportSetting>
    {
    }
    public partial class ReportSettingService:BaseService<ReportSetting>,IReportSettingService
    {
        public ReportSettingService(IUnitOfWork unitOfWork,IReportSettingRepository repository):base(unitOfWork,repository){}
    }
}
