using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Issue;
using System;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class CommentController : Controller
    {
        private readonly CommentFacade commentFacade; 

        public CommentController(CommentFacade commentFacade)
        {
            this.commentFacade = commentFacade;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(IssueDetailModel model)
        {
            var comment = new CommentDTO()
            {
                Subject = model.NewCommentSubject,
                Message = model.NewCommentMessage,
                Created = DateTime.Now
            };
            commentFacade.CreateComment(comment, model.Issue.Id, User.Identity.GetUserId<int>());
            return RedirectToAction("IssueDetail", "Issue", new { issueId = model.Issue.Id });
        }
    }
}