using DataAccessLayer;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer
{
    public abstract class AITQuery<T> : EntityFrameworkQuery<T>
    {
        public new AITDbContext Context => (AITDbContext)base.Context;

        public AITQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
