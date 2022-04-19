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
    public partial interface ISpeakerRepository :IBaseRepository<Speaker>
    {
    }
    public partial class SpeakerRepository :BaseRepository<Speaker>, ISpeakerRepository
    {
         public SpeakerRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

