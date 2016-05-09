using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;

namespace BussinesLayer.Queries
{
    public class CustomerListQuery : AITQuery<CustomerDTO>
    {
        public CustomerFilter Filter { get; set; }

        public CustomerListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<CustomerDTO> GetQueryable()
        {
            IQueryable<Customer> query = Context.Customers;
            if (Filter.CustomerId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.CustomerId);
            }
            return (Mapper.Map<List<CustomerDTO>>(query)).AsQueryable();
        }
    }
}