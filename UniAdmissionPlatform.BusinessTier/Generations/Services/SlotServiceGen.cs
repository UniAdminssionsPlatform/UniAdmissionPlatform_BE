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
    
    public partial interface ISlotService:IBaseService<Slot>
    {
    }
    public partial class SlotService:BaseService<Slot>,ISlotService
    {
        public SlotService(IUnitOfWork unitOfWork,ISlotRepository repository):base(unitOfWork,repository){}
    }
}
