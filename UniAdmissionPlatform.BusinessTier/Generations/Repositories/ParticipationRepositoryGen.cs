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
    public partial interface IParticipationRepository :IBaseRepository<Participation>
    {
    }
    public partial class ParticipationRepository :BaseRepository<Participation>, IParticipationRepository
    {
         public ParticipationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

