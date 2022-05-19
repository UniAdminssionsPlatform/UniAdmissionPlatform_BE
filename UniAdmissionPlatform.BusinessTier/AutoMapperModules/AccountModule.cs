using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class AccountModule
    {
        public static void ConfigAccountMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateAccountRequest, Account>();
            mc.CreateMap<Account, AccountBaseViewModel>();
            mc.CreateMap<UpdateProfileRequest, Account>().ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<UpdateAccountRequestForAdmin, Account>().ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
            mc.CreateMap<Account, AccountViewModelWithHighSchool>()
                .ForMember(des => des.HighSchoolBaseViewModel, opt => opt.MapFrom(
                    src => src.HighSchool))
                .ForMember(des => des.Status, opt => opt.MapFrom(
                    src => src.IdNavigation.Status));
            mc.CreateMap<AccountBaseViewModel, AccountViewModelWithHighSchool>();
            mc.CreateMap<Account, AccountViewModelWithUniversity>()
                .ForMember(des => des.UniversityBaseViewModel, opt => opt.MapFrom(
                    src => src.University));
            mc.CreateMap<AccountBaseViewModel, AccountViewModelWithUniversity>();
        }
    }
}