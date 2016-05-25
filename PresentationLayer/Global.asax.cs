using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BussinesLayer;
using Castle.Windsor;
using DataAccessLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;

namespace PresentationLayer
{
    public class Global : HttpApplication
    {
        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BootstrapContainer();
            ConfigureUserRoles();
        }

        private void ConfigureUserRoles()
        {
            const string userName = "admin";
            const string roleName = "Administrator";

            try
            {
                using (var context = new AITDbContext())
                {
                    using (var userManager = new AITUserManager(new AITUserStore(context)))
                    {
                        var user = userManager.FindByName(userName);
                        if(user == null)
                            throw new NullReferenceException($"ConfigureUserRoles() - user with username: {userName} has not been found!");

                        if (!CheckUserAlreadyHasRole(userManager, user.Id, roleName))
                            userManager.AddToRole(user.Id, roleName);
                    }
                    context.SaveChanges();
                }
            } catch(Exception ex)
            {
                Debug.WriteLine("An exception has been thrown while configuring user roles: " + ex.Message + Environment.NewLine + " ST: " + ex.StackTrace);
            }
        }

        private static bool CheckUserAlreadyHasRole(AITUserManager userManager, int userId, string roleToCheck)
        {
            var roles = userManager.GetRoles(userId);
            return roles.Contains(roleToCheck);
        }

        private static void BootstrapContainer()
        {
            container = new WindsorContainer();
            container.Install(new MvcInstaller());
            container.Install(new Installer());
            
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}