using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Customer;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class CustomerController : Controller
    {
        private readonly UserFacade userFacade;
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;

        public CustomerController(UserFacade userFacade, CustomerFacade customerFacade, ProjectFacade projectFacade)
        {
            this.userFacade = userFacade;
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
        
        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult GrantCustomerRights(int userId)
        {
            if(userFacade.IsUserCustomer(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            var model = new EditCustomerModel()
            {
                Customer = new CustomerDTO()
            };
            model.Customer.User = userFacade.GetUserById(userId);
            return View("GrantCustomerRights", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult GrantCustomerRights(EditCustomerModel model)
        {
            if (userFacade.IsUserCustomer(model.Customer.User.Id))
                RedirectToAction("UserDetail", "User", new { userId = model.Customer.User.Id });

            customerFacade.CreateCustomer(model.Customer, model.Customer.User.Id);
            userFacade.AddCustomerRightsToUser(model.Customer.User.Id);
            return RedirectToAction("UserDetail", "User", new { userId = model.Customer.User.Id });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult RemoveCustomerRights(int userId)
        {
            if(!userFacade.IsUserCustomer(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            var customer = customerFacade.GetCustomerById(userId);
            customerFacade.DeleteCustomer(customer);
            userFacade.RemoveCustomerRightsFromUser(userId);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }

        public ActionResult EditCustomer(int userId)
        {
            if(!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId))
                return View("AccessForbidden");

            var model = new EditCustomerModel()
            {
                Customer = customerFacade.GetCustomerById(userId)
            };
            return View("EditCustomer", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(EditCustomerModel model)
        {
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.Customer.Id))
                return View("AccessForbidden");

            customerFacade.UpdateCustomer(model.Customer);
            return RedirectToAction("UserDetail", "User", new { userId = model.Customer.Id });
        }
    }
}