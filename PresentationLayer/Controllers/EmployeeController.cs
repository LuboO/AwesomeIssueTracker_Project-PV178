using BussinesLayer.Facades;
using PresentationLayer.Models.Employee;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeFacade employeeFacade;

        public EmployeeController(EmployeeFacade employeeFacade)
        {
            this.employeeFacade = employeeFacade;
        }

        public ActionResult ListEmployees()
        {
            var listEmployeesModel = new ListEmployeesModel()
            {
                Employees = employeeFacade.GetAllEmployees()
            };
            return View("ListEmployees", listEmployeesModel);
        }
    }
}