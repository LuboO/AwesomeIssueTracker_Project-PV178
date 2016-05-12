using BussinesLayer.Facades;
using PresentationLayer.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;

        public CustomerController(CustomerFacade customerFacade, ProjectFacade projectFacade)
        {
            this.customerFacade = customerFacade;
            this.projectFacade = projectFacade;
        }

        public ActionResult ListCustomers()
        {
            var listCustomersModel = new ListCustomersModel()
            {
                Customers = customerFacade.GetAllCustomers()
            };
            return View("ListCustomers", listCustomersModel);
        }

        public ActionResult CustomerDetail(int customerId)
        {
            var customerDetailModel = new CustomerDetailModel()
            {
                Customer = customerFacade.GetCustomerById(customerId),
                Projects = projectFacade.GetProjectsByCustomer(customerId)
            };
            return View("CustomerDetail", customerDetailModel);
        }
    }
}