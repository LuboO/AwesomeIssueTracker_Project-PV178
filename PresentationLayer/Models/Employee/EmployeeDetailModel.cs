using BussinesLayer.DTOs;
using PresentationLayer.Models.Issue;
using System.Collections.Generic;

namespace PresentationLayer.Models.Employee
{
    public class EmployeeDetailModel
    {
        public ListIssuesModel ListIssuesModel { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}