using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Employee;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        private readonly UserFacade userFacade;
        private readonly EmployeeFacade employeeFacade;
        private readonly IssueFacade issueFacade;

        public EmployeeController(UserFacade userFacade, EmployeeFacade employeeFacade, IssueFacade issueFacade)
        {
            this.userFacade = userFacade;
            this.employeeFacade = employeeFacade;
            this.issueFacade = issueFacade;
        }

        public ActionResult ViewAllEmployees()
        {
            var model = new ViewAllEmployeesModel()
            {
                Employees = employeeFacade.GetAllEmployees()
            };
            return View("ViewAllEmployees", model);
        }
        
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult GrantEmployeeRights(int userId)
        {
            if (userFacade.IsUserEmployee(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            employeeFacade.CreateEmployee(new EmployeeDTO(), userId);
            userFacade.AddEmployeeRightsToUser(userId);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }
        
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult RemoveEmployeeRights(int userId)
        {
            if (!userFacade.IsUserEmployee(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            var employee = employeeFacade.GetEmployeeById(userId);
            userFacade.RemoveEmployeeRightFromUser(userId);
            employeeFacade.DeleteEmployee(employee);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }
    }
}