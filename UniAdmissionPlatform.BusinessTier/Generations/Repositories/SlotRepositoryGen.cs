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
    public partial interface ISlotRepository :IBaseRepository<Slot>
    {
    }
    public partial class SlotRepository :BaseRepository<Slot>, ISlotRepository
    {
         public SlotRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

