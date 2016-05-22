using DataAccessLayer;
using Riganti.Utils.Infrastructure.Core;

namespace BussinesLayer
{
    public interface IAITUnitOfWork : IUnitOfWork
    {
        AITDbContext AITDbContext { get; }
    }
}
