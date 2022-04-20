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
            mc.CreateMap<UpdateAccountRequest, Account>().ForAllMembers(opt => opt.Condition((src,des,srcMember)=> srcMember != null));
        }
    }
}