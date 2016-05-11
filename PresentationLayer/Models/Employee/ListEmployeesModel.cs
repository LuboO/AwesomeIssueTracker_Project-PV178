using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Employee
{
    public class ListEmployeesModel
    {
        public List<EmployeeDTO> Employees { get; set; }

        public ListEmployeesModel()
        {
            Employees = new List<EmployeeDTO>();
        }
    }
}