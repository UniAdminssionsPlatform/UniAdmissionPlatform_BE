using System;
using System.Security.Cryptography;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class EventCheckModule
    {
        public static void ConfigEventCheckMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<EventCheck, EventBySlotBaseViewModel>()
                .ForMember(des => des.Event,
                    opt => opt.MapFrom(src => src.Event))
                .ForMember(des => des.SlotId, 
                    opt => opt.MapFrom(src => src.SlotId));

            mc.CreateMap<EventCheck, EventWithSlotViewModel>()
                .ForMember(des => des.Event,
                    opt => opt.MapFrom(
                        src => src.Event))
                .ForMember(des => des.Slot,
                    opt => opt.MapFrom(
                        src => src.Slot));
        }
    }
}