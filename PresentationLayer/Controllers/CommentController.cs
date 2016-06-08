using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Comment;
using System;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class CommentController : Controller
    {
        private readonly CommentFacade commentFacade;
        private readonly IssueFacade issueFacade;

        public CommentController(CommentFacade commentFacade, IssueFacade issueFacade)
        {
            this.commentFacade = commentFacade;
            this.issueFacade = issueFacade;
        }

        public ActionResult PostComment(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            var issue = issueFacade.GetIssueById(issueId.Value);
            if (issue == null)
                return View("BadInput");

            var model = new PostCommentModel()
            {
                IssueId = issue.Id
            };

            return View("PostComment", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(PostCommentModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            var comment = new CommentDTO()
            {
                Subject = model.Subject,
                Message = model.Message,
                Created = DateTime.Now
            };
            
            commentFacade.CreateComment(comment, model.IssueId, User.Identity.GetUserId<int>());
            return RedirectToAction("IssueDetail", "Issue", new { issueId = model.IssueId });
        }
    }
}