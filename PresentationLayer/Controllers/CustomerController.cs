using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Customer;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize(Roles = "Administrator")]
    public class CustomerController : Controller
    {
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;

        public CustomerController(CustomerFacade customerFacade, ProjectFacade projectFacade)
        {
            this.customerFacade = customerFacade;
            this.projectFacade = projectFacade;
        }

        public ActionResult ViewAllCustomers()
        {
            var model = new ViewAllCustomersModel()
            {
                Customers = customerFacade.GetAllCustomers()
            };
            return View("ViewAllCustomers", model);
        }

        public ActionResult CreateCustomer()
        {
            var model = new EditCustomerModel()
            {
                Customer = new CustomerDTO()
            };
            return View("CreateCustomer", model);
        }

        [HttpPost]
        public ActionResult CreateCustomer(EditCustomerModel model)
        {
            customerFacade.CreateCustomer(model.Customer);
            return RedirectToAction("ViewAllCustomers");
        }

        public ActionResult EditCustomer(int customerId)
        {
            var model = new EditCustomerModel()
            {
                Customer = customerFacade.GetCustomerById(customerId)
            };
            return View("EditCustomer", model);
        }

        [HttpPost]
        public ActionResult EditCustomer(EditCustomerModel model)
        {
            customerFacade.UpdateCustomer(model.Customer);
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