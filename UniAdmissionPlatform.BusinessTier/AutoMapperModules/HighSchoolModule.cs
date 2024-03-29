﻿using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.HighSchool;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class HighSchoolModule
    {
        public static void ConfigHighSchoolMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateHighSchoolRequest, HighSchool>();
            mc.CreateMap<HighSchool, HighSchoolBaseViewModel>();
            mc.CreateMap<HighSchool, HighSchoolCodeViewModel>();
            mc.CreateMap<HighSchool, HighSchoolManagerCodeViewModel>();
            mc.CreateMap<HighSchool, GetHighSchoolBaseViewModel>();
            mc.CreateMap<UpdateHighSchoolProfileRequest, HighSchool>()
                .ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}