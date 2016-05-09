using DataAccessLayer;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Data.Entity;

namespace BussinesLayer
{
    public class AITUnitOfWork : EntityFrameworkUnitOfWork
    {
        public new AITDbContext Context => (AITDbContext)base.Context;

        public AITUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory, DbContextOptions options) 
            : base(provider, dbContextFactory, options)
        {
        }
    }
}
