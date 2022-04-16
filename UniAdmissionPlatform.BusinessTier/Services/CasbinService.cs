using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casbin.Adapter.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCasbin;
using UniAdmissionPlatform.BusinessTier.Requests.Casbin;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface ICasbinService
    {
        bool Enforce(string sub, string obj, string act);
        Task AddPolicy(AddPolicyRequest addPolicyRequest);
        Task RemovePolicy(RemovePolicyRequest removePolicyRequest);
        List<string> GetAllObjects();
        List<string> GetAllActions();
        List<string> GetAllSubjects();
        List<List<string>> GetPolicy();
    }

    public class CasbinService : ICasbinService
    {
        private readonly Enforcer _enforcer;
        public CasbinService(IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<CasbinDbContext<int>>()
                .UseMySQL(configuration["ConnectionStrings:DbContext"])
                .Options;
            var dbContext = new CasbinDbContext<int>(options);
            dbContext.Database.EnsureCreated();
            var efCoreAdapter = new EFCoreAdapter<int>(dbContext);
            _enforcer = new Enforcer("Resources/Casbin/rbac_model.conf", efCoreAdapter);
        }
        
        public bool Enforce(string sub, string obj, string act)
        {
            return _enforcer.Enforce(sub, obj, act);
        }

        public List<string> GetAllSubjects()
        {
            return _enforcer.GetAllSubjects();
        }
        
        public List<string> GetAllActions()
        {
            return _enforcer.GetAllActions();
        }
        
        public List<string> GetAllObjects()
        {
            return _enforcer.GetAllObjects();
        }

        public List<List<string>> GetPolicy()
        {
            return _enforcer.GetPolicy();
        }

        public async Task AddPolicy(AddPolicyRequest addPolicyRequest)
        {
            await _enforcer.AddPolicyAsync(addPolicyRequest.Subject, addPolicyRequest.Object, addPolicyRequest.Action);
        }

        public async Task RemovePolicy(RemovePolicyRequest removePolicyRequest)
        {
            await _enforcer.RemovePolicyAsync(removePolicyRequest.Subject, removePolicyRequest.Object, removePolicyRequest.Action);
        }
    }
}