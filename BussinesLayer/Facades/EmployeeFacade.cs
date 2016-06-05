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
    public class EmployeeFacade : AITBaseFacade
    {
        public EmployeeRepository EmployeeRepository { get; set; }

        public EmployeeListQuery EmployeeListQuery { get; set; }

        protected IQuery<EmployeeDTO> CreateQuery(EmployeeFilter filter)
        {
            var query = EmployeeListQuery;
            EmployeeListQuery.Filter = filter;
            return query;
        }

        public void CreateEmployee(EmployeeDTO employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Employee>(employee);
                if (created == null)
                    return;

                EmployeeRepository.Insert(created);
                uow.Commit();
            }
        }

        public EmployeeDTO GetEmployeeById(int employeeId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var employee = EmployeeRepository.GetById(employeeId);
                if (employee == null)
                    return null;

                return Mapper.Map<EmployeeDTO>(employee);
            }
        }

        public void UpdateEmployee(EmployeeDTO employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = EmployeeRepository.GetById(employee.Id);
                if (retrieved == null)
                    throw new ObjectNotFoundException("Employee hasn't been found");

                Mapper.Map(employee, retrieved);
                EmployeeRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteEmployee(EmployeeDTO employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = EmployeeRepository.GetById(employee.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Employee hasn't been found");

                EmployeeRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<EmployeeDTO> GetAllEmployees()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new EmployeeFilter())
                    .Execute()
                    .ToList();
            }
        }
    }
}
