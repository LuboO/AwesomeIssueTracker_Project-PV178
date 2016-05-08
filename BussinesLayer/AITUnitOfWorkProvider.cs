﻿using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class AITUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider
    {
        public AITUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) 
            : base(registry, dbContextFactory)
        {
        }

        protected override EntityFrameworkUnitOfWork CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AITUnitOfWork(this, dbContextFactory, options);
        }
    }
}