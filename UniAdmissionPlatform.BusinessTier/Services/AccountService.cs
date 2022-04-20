using AutoMapper;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;


namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IAccountService
    {
    }

    public partial class AccountService
    {
        private readonly IConfigurationProvider _mapper;

        public AccountService(IUnitOfWork unitOfWork, IAccountRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
    }
}