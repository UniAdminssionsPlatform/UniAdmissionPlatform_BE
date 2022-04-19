using System;
using System.Security.Cryptography;
using AutoMapper;
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
            mc.CreateMap<UpdateEventRequest, Event>().ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<int?, int>().ConvertUsing((src, des) => src ?? des);
            mc.CreateMap<bool?, bool>().ConvertUsing((src, des) => src ?? des);
            mc.CreateMap<DateTime?, DateTime>().ConvertUsing((src, des) => src ?? des);
        }
    }
}