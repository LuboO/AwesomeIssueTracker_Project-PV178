using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using BussinesLayer.Facades;
using PresentationLayer.App_Start;
using PresentationLayer.Models.User;
using Microsoft.Owin.Security;
using BussinesLayer.DTOs;
using Microsoft.AspNet.Identity;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Project;
using PresentationLayer.Filters.Authorization;
using DataAccessLayer.Enums;

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

        public ActionResult ViewAllUsers()
        {
            var model = new ViewAllUsersModel()
            {
                Users = userFacade.GetAllUsers()
            };
            return View("ViewAllUsers", model);
        }

        public ActionResult UserDetail(int userId)
        {
            /* Get personal data */
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
            model.IsDetailedUserAdmin = userFacade.IsUserAdmin(userId);
            model.IsAdmin = userFacade.IsUserAdmin(User.Identity.GetUserId<int>());
            model.IsEmployee = userFacade.IsUserEmployee(User.Identity.GetUserId<int>());
            model.CanModifyUser = model.IsAdmin || (userId == User.Identity.GetUserId<int>());

            return View("UserDetail", model);
        }
        
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditUser(int userId)
        {
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId))
                return View("AccessForbidden");

            var user = userFacade.GetUserById(userId);
            var model = new EditUserModel()
            {
                UserId = userId,
                UserName = user.UserName,
                Name = user.Name,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth
            };
            return View("EditUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserModel model)
        {
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.UserId))
                return View("AccessForbidden");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = userFacade.GetUserById(model.UserId);

            user.UserName = model.UserName;
            user.Name = model.Name;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            userFacade.UpdateUser(user);

            return RedirectToAction("UserDetail", new { userId = model.UserId });
        }

        public ActionResult DeleteUser(int userId)
        {
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId))
                return View("AccessForbidden");

            if (userId == User.Identity.GetUserId<int>())
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var user = userFacade.GetUserById(userId);
            userFacade.DeleteUser(user);
            return RedirectToAction("ViewAllUsers");
        }
        
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult GrantAdministratorRights(int userId)
        {
            if(userFacade.IsUserAdmin(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            userFacade.AddAdminRightsToUser(userId);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }

        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult RemoveAdministratorRights(int userId)
        {
            if(!userFacade.IsUserAdmin(userId))
                RedirectToAction("UserDetail", "User", new { userId = userId });

            userFacade.RemoveAdminRightsOfUser(userId);
            return RedirectToAction("UserDetail", "User", new { userId = userId });
        }
    }
}