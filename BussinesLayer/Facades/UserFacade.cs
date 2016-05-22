using AutoMapper;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;

namespace BussinesLayer.Facades
{
    public class UserFacade : AITBaseFacade
    {
        public Func<AITUserManager> UserManagerFactory { get; set; }

        public void Register(UserDTO user)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userManager = UserManagerFactory.Invoke();
                var aitUser = Mapper.Map<AITUser>(user);
                userManager.Create(aitUser, user.Password);
            }
        }

        public ClaimsIdentity Login(string email, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                using (UnitOfWorkProvider.Create())
                {
                    using (var userManager = UserManagerFactory.Invoke())
                    {
                        var user = userManager.FindByEmail(email);
                        if (user == null)
                            return null;

                        user = userManager.Find(user.UserName, password);
                        if (user == null)
                            return null;

                        return userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    }
                }
            }
        }
    }
}
