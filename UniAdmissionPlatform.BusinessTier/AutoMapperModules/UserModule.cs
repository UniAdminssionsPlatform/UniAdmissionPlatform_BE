using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Requests.User;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.AutoMapperModules
{
    public static class UserModule
    {
        public static void ConfigUserMapperModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<RegisterRequest, User>()
                .ForMember(u => u.Account, opt => opt.MapFrom(
                    src => new Account
                    {
                        FirstName = src.FirstName,
                        MiddleName = src.MiddleName,
                        LastName = src.LastName,
                        Address = src.Address,
                        PhoneNumber = src.PhoneNumber,
                        ProfileImageUrl = src.ProfileImageUrl,
                        Religion = src.Religion,
                        IdCard = src.IdCard,
                        PlaceOfBirth = src.PlaceOfBirth,
                        Nationality = src.Nationality,
                        DateOfBirth = src.DateOfBirth,
                        GenderId = src.GenderId,
                    }));
            
            mc.CreateMap<RegisterForStudentRequest, User>()
                .ForMember(u => u.Account, opt => opt.MapFrom(
                    src => new Account
                    {
                        FirstName = src.FirstName,
                        MiddleName = src.MiddleName,
                        LastName = src.LastName,
                        Address = src.Address,
                        PhoneNumber = src.PhoneNumber,
                        ProfileImageUrl = src.ProfileImageUrl,
                        Religion = src.Religion,
                        IdCard = src.IdCard,
                        PlaceOfBirth = src.PlaceOfBirth,
                        Nationality = src.Nationality,
                        DateOfBirth = src.DateOfBirth,
                        GenderId = src.GenderId,
                    }));
            
            mc.CreateMap<RegisterForSchoolManagerRequest, User>()
                .ForMember(u => u.Account, opt => opt.MapFrom(
                    src => new Account
                    {
                        FirstName = src.FirstName,
                        MiddleName = src.MiddleName,
                        LastName = src.LastName,
                        Address = src.Address,
                        PhoneNumber = src.PhoneNumber,
                        ProfileImageUrl = src.ProfileImageUrl,
                        Religion = src.Religion,
                        IdCard = src.IdCard,
                        PlaceOfBirth = src.PlaceOfBirth,
                        Nationality = src.Nationality,
                        DateOfBirth = src.DateOfBirth,
                        GenderId = src.GenderId,
                    }));
            
            mc.CreateMap<RegisterForUniversityManagerRequest, User>()
                .ForMember(u => u.Account, opt => opt.MapFrom(
                    src => new Account
                    {
                        FirstName = src.FirstName,
                        MiddleName = src.MiddleName,
                        LastName = src.LastName,
                        Address = src.Address,
                        PhoneNumber = src.PhoneNumber,
                        ProfileImageUrl = src.ProfileImageUrl,
                        Religion = src.Religion,
                        IdCard = src.IdCard,
                        PlaceOfBirth = src.PlaceOfBirth,
                        Nationality = src.Nationality,
                        DateOfBirth = src.DateOfBirth,
                        GenderId = src.GenderId,
                    }));

            mc.CreateMap<User, UserBaseViewModel>();
        }
    }
}