using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly PersonFacade personFacade;
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;

        public CustomerController(PersonFacade personFacade, CustomerFacade customerFacade, ProjectFacade projectFacade)
        {
            this.personFacade = personFacade;
            this.customerFacade = customerFacade;
            this.projectFacade = projectFacade;
        }

        public ActionResult ViewAllCustomers()
        {
            var viewAllCustomersModel = new ViewAllCustomersModel()
            {
                Customers = customerFacade.GetAllCustomers()
            };
            return View("ViewAllCustomers", viewAllCustomersModel);
        }

        public ActionResult CreateCustomer()
        {
            var editCustomerModel = new EditCustomerModel()
            {
                Customer = new CustomerDTO()
            };
            return View("CreateCustomer", editCustomerModel);
        }

        [HttpPost]
        public ActionResult CreateCustomer(EditCustomerModel editCustomerModel)
        {
            customerFacade.CreateCustomer(editCustomerModel.Customer);
            return RedirectToAction("ViewAllCustomers");
        }

        public ActionResult EditCustomer(int customerId)
        {
            var editCustomerModel = new EditCustomerModel()
            {
                Customer = customerFacade.GetCustomerById(customerId)
            };
            return View("EditCustomer", editCustomerModel);
        }

        [HttpPost]
        public ActionResult EditCustomer(EditCustomerModel editCustomerModel)
        {
            customerFacade.UpdateCustomer(editCustomerModel.Customer);
            return RedirectToAction("ViewAllCustomers");
        }

        public ActionResult DeleteCustomer(int customerId)
        {
            var customer = customerFacade.GetCustomerById(customerId);
            customerFacade.DeleteCustomer(customer);
            return RedirectToAction("ViewAllCustomers");
        }
    }
}