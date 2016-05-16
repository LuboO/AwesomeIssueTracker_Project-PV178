using BussinesLayer.Facades;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Issue;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;

        public IssueController(IssueFacade issueFacade, CommentFacade commentFacade)
        {
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
        }

        public ActionResult ViewAllIssues()
        {
            var viewAllIssuesModel = new ViewAllIssuesModel()
            {
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetAllIssues()
                }
            };
            return View("ViewAllIssues", viewAllIssuesModel);
        }

        public ActionResult IssueDetail(int issueId)
        {
            var issueDetailModel = new IssueDetailModel()
            {
                Issue = issueFacade.GetIssueById(issueId),
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByIssue(issueId)
                }
            };
            return View("IssueDetail", issueDetailModel);
        }
    }
}