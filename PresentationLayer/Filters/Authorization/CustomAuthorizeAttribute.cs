using System.Web.Mvc;

namespace PresentationLayer.Filters.Authorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "AccessForbidden" };
        }
    }
}