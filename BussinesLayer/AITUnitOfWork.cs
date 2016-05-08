using DataAccessLayer;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
