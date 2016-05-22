using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Data.Entity;

namespace BussinesLayer
{
    public class AITUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider, IAITUnitOfWorkProvider
    {
        public AITUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) 
            : base(registry, dbContextFactory)
        {
        }

        protected override EntityFrameworkUnitOfWork CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AITUnitOfWork(this, dbContextFactory, options);
        }

        IAITUnitOfWork IAITUnitOfWorkProvider.Create(DbContextOptions options)
        {
            return (IAITUnitOfWork)base.Create(options);
        }

        IAITUnitOfWork IAITUnitOfWorkProvider.Create()
        {
            return (IAITUnitOfWork)base.Create();
        }
    }
}
