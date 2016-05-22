using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer
{
    public interface IAITUnitOfWorkProvider : IUnitOfWorkProvider
    {
        IAITUnitOfWork Create(DbContextOptions options);
        new IAITUnitOfWork Create();
        new IUnitOfWork GetCurrent();
    }
}
