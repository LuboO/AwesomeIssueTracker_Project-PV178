using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Employee
{
    public class ViewAllEmployeesModel
    {
        public List<EmployeeDTO> Employees { get; set; }

        public ViewAllEmployeesModel()
        {
            Employees = new List<EmployeeDTO>();
        }
    }
}