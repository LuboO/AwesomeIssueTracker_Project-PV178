using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;

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
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Employee>(employee);
                EmployeeRepository.Insert(created);
                uow.Commit();
            }
        }

        public EmployeeDTO GetEmployeeById(int employeeId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var employee = EmployeeRepository
                    .GetById(employeeId);
                return Mapper.Map<EmployeeDTO>(employee);
            }
        }

        public void UpdateEmployee(EmployeeDTO employee)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = EmployeeRepository.GetById(employee.Id);
                Mapper.Map(employee, retrieved);
                EmployeeRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteEmployee(EmployeeDTO employee)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = EmployeeRepository.GetById(employee.Id);
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
