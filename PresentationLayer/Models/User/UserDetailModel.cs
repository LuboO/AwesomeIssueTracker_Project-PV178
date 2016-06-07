using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Issue;

namespace PresentationLayer.Models.User
{
    public class UserDetailModel
    {
        public EmployeeDetailModel EmployeeDetailModel { get; set; }

        public CustomerDetailModel CustomerDetailModel { get; set; }

        public ListIssuesModel ListIssuesModel { get; set; }

        public ListCommentsModel ListCommentsModel { get; set; }

        public UserDTO User { get; set; }

        public bool IsDetailedUserAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsEmployee { get; set; }

        public bool CanModifyUser { get; set; }
    }
}