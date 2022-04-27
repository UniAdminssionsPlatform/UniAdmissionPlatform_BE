using System;
using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Requests.MajorGroup;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class MajorGroupModule
    {
        public static void ConfigMajorGroupMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<MajorGroup, MajorGroupBaseViewModel>();
            mc.CreateMap<CreateMajorGroupRequest, MajorGroup>();
            mc.CreateMap<UpdateMajorGroupRequest, MajorGroup>();
            
        }
    }
}