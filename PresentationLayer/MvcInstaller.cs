﻿using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace PresentationLayer
{
    public class MvcInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
                (
                    Classes
                        .FromThisAssembly()
                        .BasedOn<IController>()
                        .LifestyleTransient()
                );
        }
    }
}