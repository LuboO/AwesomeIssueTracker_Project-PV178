using BussinesLayer.Facades;
using PresentationLayer.Models.Employee;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeFacade employeeFacade;
        private readonly IssueFacade issueFacade;

        public EmployeeController(EmployeeFacade employeeFacade, IssueFacade issueFacade)
        {
            this.employeeFacade = employeeFacade;
            this.issueFacade = issueFacade;
        }

        public ActionResult ListEmployees()
        {
            var listEmployeesModel = new ListEmployeesModel()
            {
                Employees = employeeFacade.GetAllEmployees()
            };
            return View("ListEmployees", listEmployeesModel);
        }

        public ActionResult EmployeeDetail(int employeeId)
        {
            var employeeDetailModel = new EmployeeDetailModel()
            {
                Employee = employeeFacade.GetEmployeeById(employeeId),
                AssignedIssues = issueFacade.GetIssuesByAssignedEmployee(employeeId)
            };
            return View("EmployeeDetail", employeeDetailModel);
        }
    }
}