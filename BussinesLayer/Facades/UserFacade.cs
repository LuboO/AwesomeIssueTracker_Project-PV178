using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Filters;
using BussinesLayer.Queries;
using BussinesLayer.Repositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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

        public UserListQuery UserListQuery { get; set; }

        protected IQuery<UserDTO> CreateQuery(UserFilter filter)
        {
            var query = UserListQuery;
            UserListQuery.Filter = filter;
            return query;
        }

        public void Create(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var created = Mapper.Map<AITUser>(user);
                    if (created == null)
                        return;

                    var result = userManager.Create(created, user.Password);
                    if (!result.Succeeded)
                        throw new UpdateException(result.Errors.FirstOrDefault());
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
            if (user == null)
                throw new ArgumentNullException("user");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var retrieved = userManager.FindById(user.Id);
                    if (retrieved == null)
                        throw new ObjectNotFoundException("User haven't been found");

                    Mapper.Map(user, retrieved);
                    var result = userManager.Update(retrieved);
                    if (!result.Succeeded)
                        throw new UpdateException(result.Errors.First());

                    uow.Commit();
                }
            }
        }

        public void DeleteUser(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var deleted = userManager.FindById(user.Id);
                    if (deleted == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    var context = (uow as AITUnitOfWork).Context;

                    var comments = context.Comments
                        .Where(c => c.AuthorId == user.Id)
                        .ToList();
                    CommentRepository.Delete(comments);

                    var notifications = context.Notifications
                        .Where(n => n.UserId == user.Id)
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
                    uow.Commit();

                    userManager.Delete(deleted);
                    uow.Commit();
                }
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new UserFilter())
                    .Execute()
                    .ToList();
            }
        }

        public List<UserDTO> GetUsersByName(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new UserFilter() { Name = name })
                    .Execute()
                    .ToList();
            }
        }

        public List<UserDTO> GetUsersByUserName(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new UserFilter() { UserName = username })
                    .Execute()
                    .ToList();
            }
        }

        public List<UserDTO> GetUsersByEmail(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new UserFilter() { Email = email })
                    .Execute()
                    .ToList();
            }
        }

        public ClaimsIdentity Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

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

        public void ChangePassword(int userId, string current, string newPass)
        {
            using (var userManager = UserManagerFactory.Invoke())
            {
                var result = userManager.ChangePassword(userId, current, newPass);
                if (!result.Succeeded)
                    throw new UpdateException(result.Errors.FirstOrDefault());
            }
        }

        public bool IsUserAdmin(int userId)
        {
            return IsUserInRole(userId, UserRole.Administrator.ToString());
        }

        public bool IsUserEmployee(int userId)
        {
            return IsUserInRole(userId, UserRole.Employee.ToString());
        }

        public bool IsUserCustomer(int userId)
        {
            return IsUserInRole(userId, UserRole.Customer.ToString());
        }

        private bool IsUserInRole(int userId, string role)
        {
            using (UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var user = userManager.FindById(userId);
                    if (user == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    return CheckUserAlreadyHasRole(userManager, userId, role);
                }
            }
        }

        public void AddAdminRightsToUser(int userId)
        {
            AddRightsToUser(userId, UserRole.Administrator.ToString());
        }

        public void RemoveAdminRightsOfUser(int userId)
        {
            RemoveRightsFromUser(userId, UserRole.Administrator.ToString());
        }

        public void AddEmployeeRightsToUser(int userId)
        {
            AddRightsToUser(userId, UserRole.Employee.ToString());
        }

        public void RemoveEmployeeRightFromUser(int userId)
        {
            RemoveRightsFromUser(userId, UserRole.Employee.ToString());
        }

        public void AddCustomerRightsToUser(int userId)
        {
            AddRightsToUser(userId, UserRole.Customer.ToString());
        }

        public void RemoveCustomerRightsFromUser(int userId)
        {
            RemoveRightsFromUser(userId, UserRole.Customer.ToString());
        }

        private void AddRightsToUser(int userId, string role)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var user = userManager.FindById(userId);
                    if (user == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    if (!CheckUserAlreadyHasRole(userManager, user.Id, role))
                    {
                        userManager.AddToRole(user.Id, role);
                        uow.Commit();
                    }

                }
            }
        }

        private void RemoveRightsFromUser(int userId, string role)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var user = userManager.FindById(userId);
                    if (user == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    if (CheckUserAlreadyHasRole(userManager, user.Id, role))
                    {
                        userManager.RemoveFromRole(user.Id, role);
                        uow.Commit();
                    }
                }
            }
        }

        private static bool CheckUserAlreadyHasRole(AITUserManager userManager, int userId, string roleToCheck)
        {
            var roles = userManager.GetRoles(userId);
            return roles.Contains(roleToCheck);
        }
    }
}
