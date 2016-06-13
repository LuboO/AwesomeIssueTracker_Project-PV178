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
using System.Linq;

namespace PresentationLayer.Controllers
{
    [HandleError]
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
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            var identity = userFacade.Login(model.Email, model.Password);
            if (identity == null)
            {
                ModelState.AddModelError("", "Bad E-Mail or Password");
                return View(model);
            }

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            return View("ChangePassword", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            var user = userFacade.GetUserById(User.Identity.GetUserId<int>());
            var identity = userFacade.Login(user.Email, model.CurrentPassword);
            if(identity == null)
            {
                ModelState.AddModelError("CurrentPassword", "Wrong password");
                return View(model);
            }
            userFacade.ChangePassword(user.Id, model.CurrentPassword, model.NewPassword);
            return RedirectToAction("UserDetail", new { userId = user.Id });
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View("Register", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            var user = new UserDTO()
            {
                UserName = model.UserName,
                Name = model.Name,
                Email =  model.Email,
                Password = model.Password,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth
            };

            /* Yes, this would also fail in facade. But it is harder to get nice message from there :) */
            if (!user.UserName.All(c => char.IsLetterOrDigit(c) && c < 128))
            {
                ModelState.AddModelError("UserName", "User Name can contain only alphanumeric symbols.");
                return View(model);
            }
            if(userFacade.GetUsersByUserName(user.UserName).Count > 0)
            {
                ModelState.AddModelError("UserName", "User Name is already taken.");
                return View(model);
            }
            if(userFacade.GetUsersByEmail(user.Email).Count > 0)
            {
                ModelState.AddModelError("Email", "E-Mail is already in use.");
                return View(model);
            }
            userFacade.Create(user);
            return RedirectToAction("Login");
        }

        public ActionResult ViewAllUsers()
        {
            var model = new ViewAllUsersModel()
            {
                Users = userFacade.GetAllUsers()
            };
            return View("ViewAllUsers", model);
        }

        public ActionResult UserDetail(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            /* Get personal data */
            var user = userFacade.GetUserById(userId.Value);
            if(user == null)
                return View("BadInput");

            var model = new UserDetailModel()
            {
                User = user,
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByCreator(user.Id)
                },
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByAuthor(user.Id)
                }
            };
            /* Get employee data if any */
            var employee = employeeFacade.GetEmployeeById(user.Id);
            if (employee != null)
            {
                model.EmployeeDetailModel = new EmployeeDetailModel()
                {
                    Employee = employee,
                    ListIssuesModel = new ListIssuesModel()
                    {
                        Issues = issueFacade.GetIssuesByAssignedEmployee(user.Id)
                    }
                };
            }
            /* Get customer data if any */
            var customer = customerFacade.GetCustomerById(user.Id);
            if (customer != null)
            {
                model.CustomerDetailModel = new CustomerDetailModel()
                {
                    Customer = customer,
                    Projects = projectFacade.GetProjectsByCustomer(user.Id)
                        .Select(p => new ProjectOverviewModel()
                        {
                            ProjectId = p.Id,
                            ProjectName = p.Name,
                            CustomerId = p.Customer.Id,
                            CustomerName = p.Customer.User.Name,
                            ErrorCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Error).Count,
                            RequirementCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Requirement).Count
                        })
                        .ToList()
                };
            }
            model.IsDetailedUserAdmin = userFacade.IsUserAdmin(user.Id);
            model.IsAdmin = userFacade.IsUserAdmin(User.Identity.GetUserId<int>());
            model.IsEmployee = userFacade.IsUserEmployee(User.Identity.GetUserId<int>());
            model.CanModifyUser = model.IsAdmin || (user.Id == User.Identity.GetUserId<int>());
            model.CanChangePassword = user.Id == User.Identity.GetUserId<int>();

            return View("UserDetail", model);
        }
        
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditUser(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId))
                return View("AccessForbidden");

            var user = userFacade.GetUserById(userId.Value);
            if (user == null)
                return View("BadInput");

            var model = new EditUserModel()
            {
                UserId = user.Id,
                Email = user.Email,
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
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.UserId))
                return View("AccessForbidden");

            var user = userFacade.GetUserById(model.UserId);
            if(user == null)
                return View("BadInput");

            if (!model.UserName.All(c => char.IsLetterOrDigit(c) && c < 128))
            {
                ModelState.AddModelError("UserName", "User Name can contain only alphanumeric symbols.");
                return View(model);
            }
            if ((user.UserName != model.UserName) && (userFacade.GetUsersByUserName(model.UserName).Count > 0))
            {
                ModelState.AddModelError("UserName", "User Name is already taken.");
                return View(model);
            }
            if ((user.Email != model.Email) && (userFacade.GetUsersByEmail(model.Email).Count > 0))
            {
                ModelState.AddModelError("Email", "E-Mail is already in use.");
                return View(model);
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;

            userFacade.UpdateUser(user);
            return RedirectToAction("UserDetail", new { userId = model.UserId });
        }

        public ActionResult DeleteUser(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != userId.Value))
                return View("AccessForbidden");

            if (userId.Value == User.Identity.GetUserId<int>())
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var user = userFacade.GetUserById(userId.Value);
            if(user == null)
                return View("BadInput");
            
            userFacade.DeleteUser(user);
            return RedirectToAction("ViewAllUsers");
        }
        
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult GrantAdministratorRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if(userFacade.GetUserById(userId.Value) == null)
                return View("BadInput");

            if (userFacade.IsUserAdmin(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            userFacade.AddAdminRightsToUser(userId.Value);
            return RedirectToAction("UserDetail", "User", new { userId = userId.Value });
        }

        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult RemoveAdministratorRights(int? userId)
        {
            if (!userId.HasValue)
                return View("BadInput");

            if (userFacade.GetUserById(userId.Value) == null)
                return View("BadInput");

            if (!userFacade.IsUserAdmin(userId.Value))
                RedirectToAction("UserDetail", "User", new { userId = userId.Value });

            userFacade.RemoveAdminRightsOfUser(userId.Value);
            return RedirectToAction("UserDetail", "User", new { userId = userId.Value });
        }
    }
}