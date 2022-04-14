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
    public partial interface INewsMajorRepository :IBaseRepository<NewsMajor>
    {
    }
    public partial class NewsMajorRepository :BaseRepository<NewsMajor>, INewsMajorRepository
    {
         public NewsMajorRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

