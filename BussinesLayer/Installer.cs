using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DataAccessLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System;
using System.Data.Entity;

namespace BussinesLayer
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register 
            (
                Component
                    .For<Func<DbContext>>()
                    .Instance(() => new AITDbContext())
                    .LifestyleTransient(),

                Component
                    .For<IUnitOfWorkProvider>()
                    .ImplementedBy<AITUnitOfWorkProvider>()
                    .LifestyleSingleton(),

                Component
                    .For<IUnitOfWorkRegistry>()
                    .Instance(new HttpContextUnitOfWorkRegistry(new ThreadLocalUnitOfWorkRegistry()))
                    .LifestyleSingleton(),

                Classes
                    .FromAssemblyContaining<AITUnitOfWork>()
                    .BasedOn(typeof(AITQuery<>))
                    .LifestyleTransient(),

                Classes
                    .FromAssemblyContaining<AITUnitOfWork>()
                    .BasedOn(typeof(IRepository<,>))
                    .LifestyleTransient(),

                Component
                    .For(typeof(IRepository<,>))
                    .ImplementedBy(typeof(EntityFrameworkRepository<,>))
                    .LifestyleTransient(),

                Classes
                    .FromAssemblyContaining<AITBaseFacade>()
                    .BasedOn<AITBaseFacade>()
                    .LifestyleTransient(),

                Component
                    .For<Func<AITUserManager>>()
                    .Instance(() => new AITUserManager(new AITUserStore(new AITDbContext())))
                    .LifestyleTransient()
            );

            Mapping.Create();
        }
    }
}
