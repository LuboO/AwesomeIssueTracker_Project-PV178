using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;
using BussinesLayer.Filters;
using System;
using System.Data.Entity.Core;
using Microsoft.AspNet.Identity;

namespace BussinesLayer.Facades
{
    public class CustomerFacade : AITBaseFacade
    {
        public Func<AITUserManager> UserManagerFactory { get; set; }

        public CustomerRepository CustomerRepository { get; set; }

        public CustomerListQuery CustomerListQuery { get; set; }

        protected IQuery<CustomerDTO> CreateQuery(CustomerFilter filter)
        {
            var query = CustomerListQuery;
            CustomerListQuery.Filter = filter;
            return query;
        }

        public int CreateCustomer(CustomerDTO customer, int userId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var created = Mapper.Map<Customer>(customer);

                    if (userManager.FindById(userId) == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    created.Id = userId;
                    created.User = null;

                    CustomerRepository.Insert(created);
                    uow.Commit();
                    return created.Id;
                }
            }
        }

        public CustomerDTO GetCustomerById(int customerId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var customer = CustomerRepository.GetById(customerId);
                if (customer == null)
                    return null;

                return Mapper.Map<CustomerDTO>(customer);
            }
        }

        public void UpdateCustomer(CustomerDTO customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = CustomerRepository.GetById(customer.Id);
                if (retrieved == null)
                    throw new ObjectNotFoundException("Customer wasn't found");

                Mapper.Map(customer, retrieved);
                CustomerRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteCustomer(CustomerDTO customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = CustomerRepository.GetById(customer.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Customer wasn't found");

                CustomerRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<CustomerDTO> GetAllCustomers()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new CustomerFilter())
                    .Execute()
                    .ToList();
            }
        }
    }
}
