using System;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class EventModule
    {
        public static void ConfigEventMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateEventRequest, Event>();
            mc.CreateMap<Event, EventBaseViewModel>();
            mc.CreateMap<Event, EventBySlotBaseViewModel>();
            mc.CreateMap<UpdateEventRequest, Event>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<Event, EventByUniIdBaseViewModel>();
            mc.CreateMap<int?, int>().ConvertUsing((src, des) => src ?? des);
            mc.CreateMap<bool?, bool>().ConvertUsing((src, des) => src ?? des);
            mc.CreateMap<DateTime?, DateTime>().ConvertUsing((src, des) => src ?? des);
            mc.CreateMap<Event, EventWithStatusInSlotViewModel>();
            mc.CreateMap<Event, EventWithSlotModel>()
                .ForMember(des => des.Slots,
                    opt => opt.MapFrom(
                        src => src.EventChecks));
            mc.CreateMap<Event, EventWithIsApproveModel>()
                .ForMember(des => des.IsApprove, opt =>
                    opt.MapFrom(src =>
                        src.EventTypeId != 2
                            ? (bool?)null
                            : src.EventChecks.Any(ec => ec.Status == (int)EventCheckStatus.Approved)));
        }
    }
}