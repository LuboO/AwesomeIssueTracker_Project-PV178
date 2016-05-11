using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using BussinesLayer.Filters;
using AutoMapper.QueryableExtensions;

namespace BussinesLayer.Queries
{
    public class EmployeeListQuery : AITQuery<EmployeeDTO>
    {
        public EmployeeFilter Filter { get; set; }

        public EmployeeListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<EmployeeDTO> GetQueryable()
        {
            IQueryable<Employee> query = Context.Employees;

            if (Filter.EmployeeId != null)
                query = query
                    .Where(e => e.Id == Filter.EmployeeId);

            return query.Project().To<EmployeeDTO>();
        }
    }
}