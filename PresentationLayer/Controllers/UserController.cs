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

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly UserFacade userFacade;

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

        public UserController(UserFacade userFacade)
        {
            this.userFacade = userFacade;
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
                //var user = new UserDTO
                //{
                //    UserName = model.UserName,
                //    Email = model.Email,
                //    Password = model.Password,
                //};
                model.User.Password = model.Password;
                userFacade.Register(model.User);

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}