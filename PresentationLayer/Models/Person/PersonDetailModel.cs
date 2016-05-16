using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Issue;
using System.Collections.Generic;

namespace PresentationLayer.Models.Person
{
    public class PersonDetailModel
    {
        public EmployeeDetailModel EmployeeDetailModel { get; set; }
        public CustomerDetailModel CustomerDetailModel { get; set; }
        public ListIssuesModel ListIssuesModel { get; set; }
        public ListCommentsModel ListCommentsModel { get; set; }
        public PersonDTO Person { get; set; }
    }
}