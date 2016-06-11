using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Issue;
using System;
using System.Data.Entity.Core;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class IssueController : Controller
    {
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;
        private readonly ProjectFacade projectFacade;
        private readonly UserFacade userFacade;
        private readonly EmployeeFacade employeeFacade;
        private readonly NotificationFacade notificationFacade;

        public IssueController(
            IssueFacade issueFacade, 
            CommentFacade commentFacade, 
            ProjectFacade projectFacade,
            UserFacade userFacade,
            EmployeeFacade employeeFacade,
            NotificationFacade notificationFacade)
        {
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
            this.projectFacade = projectFacade;
            this.userFacade = userFacade;
            this.employeeFacade = employeeFacade;
            this.notificationFacade = notificationFacade;
        }

        public ActionResult ViewAllIssues()
        {
            var model = new ViewAllIssuesModel()
            {
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetAllIssues()
                }
            };
            return View("ViewAllIssues", model);
        }

        public ActionResult IssueDetail(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            var model = new IssueDetailModel()
            {
                Issue = issue,
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByIssue(issue.Id)
                }
            };
            int userId = User.Identity.GetUserId<int>();

            model.IsUserSubscribed = notificationFacade.IsUserSubbedToIssue(userId, issueId.Value);
            model.CanChangeState = User.IsInRole(UserRole.Administrator.ToString()) || 
                User.IsInRole(UserRole.Employee.ToString());
            model.CanModify = User.IsInRole(UserRole.Administrator.ToString()) || 
                (userId == issue.Creator.Id);

            return View("IssueDetail", model);
        }

        public ActionResult AddIssueToProject(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var model = new EditIssueModel()
            {
                ProjectId = projectId.Value,
                ExistingEmployees = employeeFacade.GetAllEmployees()
            };
            return View("AddIssueToProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIssueToProject(EditIssueModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
            {
                model.ExistingEmployees = employeeFacade.GetAllEmployees();
                return View(model);
            }

            var issue = new IssueDTO()
            {
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
                Status = IssueStatus.New,
                Created = DateTime.Now
            };
            var newId = issueFacade.CreateIssue(issue, model.ProjectId, User.Identity.GetUserId<int>(), model.SelectedEmployeeId);
            return RedirectToAction("IssueDetail", new { issueId = newId });
        }

        public ActionResult EditIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != issue.Creator.Id))
                return View("AccessForbidden");

            var model = new EditIssueModel()
            {
                IssueId = issue.Id,
                CreatorId = issue.Creator.Id,
                ProjectId = issue.Project.Id,
                SelectedEmployeeId = issue.AssignedEmployee.Id,
                ExistingEmployees = employeeFacade.GetAllEmployees(),
                Title = issue.Title,
                Description = issue.Description,
                Type = issue.Type
            };
            return View("EditIssue", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIssue(EditIssueModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
            {
                model.ExistingEmployees = employeeFacade.GetAllEmployees();
                return View(model);
            }

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.CreatorId))
                return View("AccessForbidden");

            var issue = issueFacade.GetIssueById(model.IssueId);
            if(issue == null)
                return View("BadInput");

            issue.Title = model.Title;
            issue.Description = model.Description;
            issue.Type = model.Type;
            issueFacade.UpdateIssue(issue, model.SelectedEmployeeId, User.Identity.Name/*model.ProjectId, model.CreatorId, model.SelectedEmployeeId*/);
            return RedirectToAction("IssueDetail", new { issueId = model.IssueId });
        }

        public ActionResult DeleteIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != issue.Creator.Id))
                return View("AccessForbidden");

            issueFacade.DeleteIssue(issue);
            return RedirectToAction("ViewAllIssues");
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult AcceptIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            /*var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            issue.Status = IssueStatus.Accepted;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);*/
            issueFacade.AcceptIssue(issueId.Value, User.Identity.Name);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult RejectIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            /*var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            issue.Status = IssueStatus.Rejected;
            issue.Finished = DateTime.Now;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);*/
            issueFacade.RejectIssue(issueId.Value, User.Identity.Name);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult CloseIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            /*var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            issue.Status = IssueStatus.Closed;
            issue.Finished = DateTime.Now;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);*/
            issueFacade.CloseIssue(issueId.Value, User.Identity.Name);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult ReopenIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            /*var issue = issueFacade.GetIssueById(issueId.Value);
            if(issue == null)
                return View("BadInput");

            issue.Status = IssueStatus.Accepted;
            issue.Finished = null;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);*/
            issueFacade.ReopenIssue(issueId.Value, User.Identity.Name);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }
    }
}