﻿using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Issue;
using System;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class IssueController : Controller
    {
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;
        private readonly ProjectFacade projectFacade;
        private readonly UserFacade userFacade;
        private readonly EmployeeFacade employeeFacade;

        public IssueController(
            IssueFacade issueFacade, 
            CommentFacade commentFacade, 
            ProjectFacade projectFacade,
            UserFacade userFacade,
            EmployeeFacade employeeFacade)
        {
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
            this.projectFacade = projectFacade;
            this.userFacade = userFacade;
            this.employeeFacade = employeeFacade;
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

        public ActionResult IssueDetail(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            var model = new IssueDetailModel()
            {
                Issue = issue,
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByIssue(issueId)
                }
            };
            model.CanChangeState = User.IsInRole(UserRole.Administrator.ToString()) || 
                User.IsInRole(UserRole.Employee.ToString());
            model.CanModify = User.IsInRole(UserRole.Administrator.ToString()) || 
                (User.Identity.GetUserId<int>() == issue.Creator.Id);

            return View("IssueDetail", model);
        }

        public ActionResult AddIssueToProject(int projectId)
        {
            var model = new EditIssueModel()
            {
                ProjectId = projectId,
                ExistingEmployees = employeeFacade.GetAllEmployees()
            };
            return View("AddIssueToProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIssueToProject(EditIssueModel model)
        {
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

        public ActionResult EditIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != issue.Creator.Id))
                return View("AccessForbidden");

            var model = new EditIssueModel()
            {
                IssueId = issueId,
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
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.CreatorId))
                return View("AccessForbidden");

            var issue = issueFacade.GetIssueById(model.IssueId);
            issue.Title = model.Title;
            issue.Description = model.Description;
            issue.Type = model.Type;
            issueFacade.UpdateIssue(issue, model.ProjectId, model.CreatorId, model.SelectedEmployeeId);
            return RedirectToAction("IssueDetail", new { issueId = model.IssueId });
        }

        public ActionResult DeleteIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != issue.Creator.Id))
                return View("AccessForbidden");

            issueFacade.DeleteIssue(issue);
            return RedirectToAction("ViewAllIssues");
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult AcceptIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            issue.Status = IssueStatus.Accepted;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult RejectIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            issue.Status = IssueStatus.Rejected;
            issue.Finished = DateTime.Now;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult CloseIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            issue.Status = IssueStatus.Closed;
            issue.Finished = DateTime.Now;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }

        [CustomAuthorize(Roles = "Administrator,Employee")]
        public ActionResult ReopenIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            issue.Status = IssueStatus.Accepted;
            issue.Finished = null;
            issueFacade.UpdateIssue(issue, issue.Project.Id, issue.Creator.Id, issue.AssignedEmployee.Id);
            return RedirectToAction("IssueDetail", new { issueId = issueId });
        }
    }
}