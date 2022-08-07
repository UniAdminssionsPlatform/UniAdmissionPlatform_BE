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
            mc.CreateMap<EventCheck, EventWithStatusInSlotViewModel>()
                .ForMember(des => des.Event, opt =>
                    opt.MapFrom(src => src.Event))
                .ForMember(des => des.StatusInSlot, opt =>
                    opt.MapFrom(src => src.Status));

            mc.CreateMap<Slot, SlotWithEventCheckStatusModel>()
                .ForMember(des => des.HighSchoolName, opt => opt.MapFrom(
                    src => src.HighSchool.Name))
                .ForMember(des => des.HighSchoolAddress, opt => opt.MapFrom(
                    src => src.HighSchool.Address))
                .ForMember(des => des.HighSchoolId, opt => opt.MapFrom(
                    src => src.HighSchool.Id));

            mc.CreateMap<EventCheck, SlotWithEventCheckStatusModel>()
                .ForMember(des => des.Id, opt =>
                    opt.MapFrom(src => src.Slot.Id))
                .ForMember(des => des.StartDate, opt =>
                    opt.MapFrom(src => src.Slot.StartDate))
                .ForMember(des => des.EndDate, opt =>
                    opt.MapFrom(src => src.Slot.EndDate))
                .ForMember(des => des.HighSchoolId, opt =>
                    opt.MapFrom(src => src.Slot.HighSchoolId))
                .ForMember(des => des.EventCheckStatus, opt =>
                    opt.MapFrom(src => src.Status))
                .ForMember(des => des.Status, opt =>
                    opt.MapFrom(src => src.Slot.Status))
                .ForMember(des => des.HighSchoolName, opt =>
                    opt.MapFrom(src => src.Slot.HighSchool.Name))
                .ForMember(des => des.HighSchoolAddress, opt =>
                    opt.MapFrom(src => src.Slot.HighSchool.Address));
            mc.CreateMap<EventCheck, EventCheckWithEventAndSlotModel>()
                .ForMember(des => des.Event,
                    opt => opt.MapFrom(
                        src => src.Event))
                .ForMember(des => des.Slot,
                    opt => opt.MapFrom(
                        src => src.Slot));
        }
    }
}