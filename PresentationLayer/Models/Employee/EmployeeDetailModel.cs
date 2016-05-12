using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Employee
{
    public class EmployeeDetailModel
    {
        public EmployeeDTO Employee { get; set; }

        public List<IssueDTO> AssignedIssues { get; set; }

        public EmployeeDetailModel()
        {
            AssignedIssues = new List<IssueDTO>();
        }
    }
}