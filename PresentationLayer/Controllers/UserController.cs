using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using BussinesLayer.Facades;
using PresentationLayer.App_Start;
using PresentationLayer.Models.User;
using System.Net;
using Microsoft.Owin.Security;
using BussinesLayer.DTOs;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Project;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly UserFacade userFacade;
        private readonly EmployeeFacade employeeFacade;
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UserController(
            UserFacade userFacade, EmployeeFacade employeeFacade, CustomerFacade customerFacade,
            ProjectFacade projectFacade, IssueFacade issueFacade, CommentFacade commentFacade)
        {
            this.userFacade = userFacade;
            this.employeeFacade = employeeFacade;
            this.customerFacade = customerFacade;
            this.projectFacade = projectFacade;
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var identity = userFacade.Login(model.Email, model.Password);
            if (identity != null)
            {
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Bad E-Mail or Password");
                return View(model);
            }
        }

        public ActionResult UserDetail()
        {
            /* Get personal data */
            int userId = User.Identity.GetUserId<int>();
            var model = new UserDetailModel()
            {
                User = userFacade.GetUserById(userId),
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByCreator(userId)
                },
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByAuthor(userId)
                }
            };
            /* Get employee data if any */
            var employee = employeeFacade.GetEmployeeById(userId);
            if (employee != null)
            {
                model.EmployeeDetailModel = new EmployeeDetailModel()
                {
                    Employee = employee,
                    ListIssuesModel = new ListIssuesModel()
                    {
                        Issues = issueFacade.GetIssuesByAssignedEmployee(userId)
                    }
                };
            }
            /* Get customer data if any */
            var customer = customerFacade.GetCustomerById(userId);
            if (customer != null)
            {
                model.CustomerDetailModel = new CustomerDetailModel()
                {
                    Customer = customer,
                    ListProjectsModel = new ListProjectsModel()
                    {
                        Projects = projectFacade.GetProjectsByCustomer(userId)
                    }
                };
            }
            return View("UserDetail", model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel()
            {
                User = new UserDTO()
            };
            return View("Register", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.User.Password = model.Password;
                userFacade.Create(model.User);

                return RedirectToAction("Index", "Home");
            }
            
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditAccount()
        {
            int userId = User.Identity.GetUserId<int>();
            var user = userFacade.GetUserById(userId);
            var model = new EditAccountModel()
            {
                UserName = user.UserName,
                Name = user.Name,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth
            };
            return View("EditAccount", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(EditAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int userId = User.Identity.GetUserId<int>();
            var user = userFacade.GetUserById(userId);
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            userFacade.UpdateUser(user);
            return RedirectToAction("UserDetail");
        }
    }
}