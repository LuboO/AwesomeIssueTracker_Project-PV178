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
        public ActionResult GrantCustomerRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (userFacade.IsUserCustomer(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            var user = userFacade.GetUserById(userId.Value);
            if (user == null)
                return View("BadInput");

            var model = new EditCustomerModel()
            {
                UserId = user.Id,
            };
            return View("GrantCustomerRights", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult GrantCustomerRights(EditCustomerModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            if (userFacade.IsUserCustomer(model.UserId))
                RedirectToAction("UserDetail", "User", new { userId = model.UserId });

            customerFacade.CreateCustomer(new CustomerDTO() { Type = model.Type }, model.UserId);
            userFacade.AddCustomerRightsToUser(model.UserId);
            return RedirectToAction("UserDetail", "User", new { userId = model.UserId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult RemoveCustomerRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (!userFacade.IsUserCustomer(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            var customer = customerFacade.GetCustomerById(userId.Value);
            if (customer == null)
                return View("BadInput");

            customerFacade.DeleteCustomer(customer);
            userFacade.RemoveCustomerRightsFromUser(userId.Value);
            return RedirectToAction("UserDetail", "User", new { userId = userId.Value });
        }

        public ActionResult EditCustomer(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId.Value))
                return View("AccessForbidden");

            var customer = customerFacade.GetCustomerById(userId.Value);
            if(customer == null)
                return View("BadInput");

            var model = new EditCustomerModel()
            {
                UserId = customer.Id,
                Type = customer.Type
            };
            return View("EditCustomer", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(EditCustomerModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.UserId))
                return View("AccessForbidden");

            var customer = customerFacade.GetCustomerById(model.UserId);
            if (customer == null)
                return View("BadInput");

            customer.Type = model.Type;

            customerFacade.UpdateCustomer(customer);
            return RedirectToAction("UserDetail", "User", new { userId = model.UserId });
        }
    }
}