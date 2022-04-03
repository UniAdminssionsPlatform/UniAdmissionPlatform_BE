using System;
using System.Threading.Tasks;

namespace UniAdmissionPlatform.DataTier.BaseConnect
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
