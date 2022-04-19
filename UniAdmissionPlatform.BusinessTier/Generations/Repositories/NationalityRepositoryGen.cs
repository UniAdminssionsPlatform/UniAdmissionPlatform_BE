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
    public partial interface INationalityRepository :IBaseRepository<Nationality>
    {
    }
    public partial class NationalityRepository :BaseRepository<Nationality>, INationalityRepository
    {
         public NationalityRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

