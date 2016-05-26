using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BussinesLayer.Facades
{
    public class UserFacade : AITBaseFacade
    {
        public Func<AITUserManager> UserManagerFactory { get; set; }

        public IssueRepository IssueRepository { get; set; }

        public CommentRepository CommentRepository { get; set; }

        public NotificationRepository NotificationRepository { get; set; }

        public EmployeeRepository EmployeeRepository { get; set; }

        public CustomerRepository CustomerRepository { get; set; }

        public ClaimsIdentity Login(string email, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var user = userManager.FindByEmail(email);
                    if (user == null)
                        return null;

                    if (!userManager.CheckPassword(user, password))
                        return null;

                    return userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                }
            }
        }

        public void Create(UserDTO user)
        {
            using (UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var aitUser = Mapper.Map<AITUser>(user);
                    userManager.Create(aitUser, user.Password);
                }
            }
        }

        public UserDTO GetUserById(int userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var user = userManager.FindById(userId);
                    if (user == null)
                        return null;
                    
                    return Mapper.Map<UserDTO>(user);
                }
            }
        }

        public void UpdateUser(UserDTO user)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var retrieved = userManager.FindById(user.Id);
                    Mapper.Map(user, retrieved);
                    userManager.Update(retrieved);
                }
                uow.Commit();
            }
        }

        public void DeleteUser(UserDTO user)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var context = ((IAITUnitOfWork)uow).AITDbContext;

                var comments = context.Comments
                    .Where(c => c.AuthorId == user.Id)
                    .ToList();
                CommentRepository.Delete(comments);

                var notifications = context.Notifications
                    .Where(n => n.PersonId == user.Id)
                    .ToList();
                NotificationRepository.Delete(notifications);

                var issues = context.Issues
                .Where(i => i.CreatorId == user.Id)
                .ToList();
                IssueRepository.Delete(issues);

                var employees = context.Employees
                    .Where(e => e.Id == user.Id)
                    .ToList();
                EmployeeRepository.Delete(employees);

                var customers = context.Customers
                    .Where(c => c.Id == user.Id)
                    .ToList();
                CustomerRepository.Delete(customers);

                using (var userManager = UserManagerFactory.Invoke())
                {
                    var deleted = userManager.FindById(user.Id);
                    userManager.Delete(deleted);
                }
                uow.Commit();
            }
        }
    }
}
