using System.Net;
using System.Threading.Tasks;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IHighSchoolService
    {
        Task<HighSchool> GetHighSchoolByCode(string highSchoolCode);
    }
    
    public partial class HighSchoolService
    {
        public async Task<HighSchool> GetHighSchoolByCode(string highSchoolCode)
        {
            var highSchool = await FirstOrDefaultAsyn(hs => hs.HighSchoolCode == highSchoolCode);
            if (highSchool == null)
            {
                throw new ErrorResponse((int)(HttpStatusCode.NotFound),
                    "Không thể tìm thấy trường THPT nào ứng với mã đã cung cấp.");
            }

            return highSchool;
        }
    }
}