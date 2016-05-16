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
            var viewAllEmployeesModel = new ViewAllEmployeesModel()
            {
                Employees = employeeFacade.GetAllEmployees()
            };
            return View("ViewAllEmployees", viewAllEmployeesModel);
        }

        public ActionResult CreateEmployee()
        {
            var editEmployeeModel = new EditEmployeeModel()
            {
                Employee = new EmployeeDTO()
            };
            return View("CreateEmployee", editEmployeeModel);
        }

        [HttpPost]
        public ActionResult CreateEmployee(EditEmployeeModel editEmployeeModel)
        {
            employeeFacade.CreateEmployee(editEmployeeModel.Employee);
            return RedirectToAction("ViewAllEmployees");
        }

        public ActionResult EditEmployee(int employeeId)
        {
            var editEmployeeModel = new EditEmployeeModel
            {
                Employee = employeeFacade.GetEmployeeById(employeeId)
            };
            return View("EditEmployee", editEmployeeModel);
        }

        [HttpPost]
        public ActionResult EditEmployee(EditEmployeeModel editEmployeeModel)
        {
            employeeFacade.UpdateEmployee(editEmployeeModel.Employee);
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