using BussinesLayer.Facades;
using PresentationLayer.Models.Issue;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueFacade issueFacade;

        public IssueController(IssueFacade issueFacade)
        {
            this.issueFacade = issueFacade;
        }

        public ActionResult ListIssues()
        {
            var listIssuesModel = new ListIssuesModel()
            {
                Issues = issueFacade.GetAllIssues()
            };
            return View("ListIssues", listIssuesModel);
        }
    }
}