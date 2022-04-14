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
    public partial interface ICertificationRepository :IBaseRepository<Certification>
    {
    }
    public partial class CertificationRepository :BaseRepository<Certification>, ICertificationRepository
    {
         public CertificationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

