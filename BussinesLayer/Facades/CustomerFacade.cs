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

namespace BussinesLayer.Facades
{
    public class CustomerFacade : AITBaseFacade
    {
        public CustomerRepository CustomerRepository { get; set; }

        public CustomerListQuery CustomerListQuery { get; set; }

        protected IQuery<CustomerDTO> CreateQuery(CustomerFilter filter)
        {
            var query = CustomerListQuery;
            CustomerListQuery.Filter = filter;
            return query;
        }

        public void CreateCustomer(CustomerDTO customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Customer>(customer);
                if (created == null)
                    return;

                CustomerRepository.Insert(created);
                uow.Commit();
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
                    throw new ObjectNotFoundException("Customer hasn't been found");

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
                    throw new ObjectNotFoundException("Customer hasn't been found");

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
