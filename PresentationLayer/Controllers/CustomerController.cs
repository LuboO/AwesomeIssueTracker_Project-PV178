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

        public CustomerController(CustomerFacade customerFacade)
        {
            this.customerFacade = customerFacade;
        }

        public ActionResult ListCustomers()
        {
            var listCustomersModel = new ListCustomersModel()
            {
                Customers = customerFacade.GetAllCustomers()
            };
            return View("ListCustomers", listCustomersModel);
        }
    }
}