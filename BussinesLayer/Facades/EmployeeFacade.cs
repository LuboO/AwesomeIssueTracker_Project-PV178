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
    public class EmployeeFacade : AITBaseFacade
    {
        public Func<AITUserManager> UserManagerFactory { get; set; }

        public EmployeeRepository EmployeeRepository { get; set; }

        public EmployeeListQuery EmployeeListQuery { get; set; }

        protected IQuery<EmployeeDTO> CreateQuery(EmployeeFilter filter)
        {
            var query = EmployeeListQuery;
            EmployeeListQuery.Filter = filter;
            return query;
        }

        public int CreateEmployee(EmployeeDTO employee, int userId)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var created = Mapper.Map<Employee>(employee);
                    
                    if (userManager.FindById(userId) == null)
                        throw new ObjectNotFoundException("User wasn't found");

                    created.Id = userId;
                    created.User = null;

                    EmployeeRepository.Insert(created);
                    uow.Commit();
                    return created.Id;
                }
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
                    throw new ObjectNotFoundException("Employee wasn't found");

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
                    throw new ObjectNotFoundException("Employee wasn't found");

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
