using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Employee;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [HandleError]
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
        public ActionResult GrantEmployeeRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if(userFacade.GetUserById(userId.Value) == null)
                return View("BadInput");

            if (userFacade.IsUserEmployee(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            employeeFacade.CreateEmployee(new EmployeeDTO(), userId.Value);
            userFacade.AddEmployeeRightsToUser(userId.Value);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }
        
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult RemoveEmployeeRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (!userFacade.IsUserEmployee(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            var employee = employeeFacade.GetEmployeeById(userId.Value);
            if(employee == null)
                return View("BadInput");

            userFacade.RemoveEmployeeRightFromUser(userId.Value);
            employeeFacade.DeleteEmployee(employee);
            return RedirectToAction("UserDetail", "User", new { userId = userId.Value });
        }
    }
}