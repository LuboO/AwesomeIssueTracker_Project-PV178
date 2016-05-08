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

        public void CreateEmployee(EmployeeDTO Employee)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Employee>(Employee);
                EmployeeRepository.Insert(created);
                uow.Commit();
            }
        }

        public EmployeeDTO GetEmployeeById(int EmployeeId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Employee = EmployeeRepository
                    .GetById(EmployeeId);
                return Mapper.Map<EmployeeDTO>(Employee);
            }
        }

        public void UpdateEmployee(EmployeeDTO Employee)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = EmployeeRepository.GetById(Employee.Id);
                Mapper.Map(Employee, retrieved);
                EmployeeRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteEmployee(EmployeeDTO Employee)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = EmployeeRepository.GetById(Employee.Id);
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
