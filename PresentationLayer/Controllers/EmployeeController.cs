using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Person;
using System;
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

        public ActionResult ViewAllEmployees()
        {
            var model = new ViewAllEmployeesModel()
            {
                Employees = employeeFacade.GetAllEmployees()
            };
            return View("ViewAllEmployees", model);
        }

        public ActionResult CreateEmployee()
        {
            var model = new EditEmployeeModel()
            {
                Employee = new EmployeeDTO()
            };
            return View("CreateEmployee", model);
        }

        [HttpPost]
        public ActionResult CreateEmployee(EditEmployeeModel model)
        {
            employeeFacade.CreateEmployee(model.Employee);
            return RedirectToAction("ViewAllEmployees");
        }

        public ActionResult EditEmployee(int employeeId)
        {
            var model = new EditEmployeeModel
            {
                Employee = employeeFacade.GetEmployeeById(employeeId)
            };
            return View("EditEmployee", model);
        }

        [HttpPost]
        public ActionResult EditEmployee(EditEmployeeModel model)
        {
            employeeFacade.UpdateEmployee(model.Employee);
            return RedirectToAction("ViewAllEmployees");
        }

        public ActionResult DeleteEmployee(int employeeId)
        {
            var employee = employeeFacade.GetEmployeeById(employeeId);
            employeeFacade.DeleteEmployee(employee);
            return RedirectToAction("ViewAllEmployees");
        }
    }
}