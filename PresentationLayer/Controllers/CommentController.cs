using PresentationLayer.Filters.Authorization;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class CommentController : Controller
    {
    }
}