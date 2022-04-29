using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class SlotModule
    {
        public static void ConfigSlotMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Slot, SlotViewModel>();
            mc.CreateMap<CreateSlotRequest, Slot>();
            mc.CreateMap<UpdateSlotRequest, Slot>();
        }
    }
}