using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Issue;
using System;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;
        private readonly ProjectFacade projectFacade;
        private readonly PersonFacade personFacade;
        private readonly EmployeeFacade employeeFacade;

        public IssueController(
            IssueFacade issueFacade, 
            CommentFacade commentFacade, 
            ProjectFacade projectFacade,
            PersonFacade personFacade,
            EmployeeFacade employeeFacade)
        {
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
            this.projectFacade = projectFacade;
            this.personFacade = personFacade;
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
            var model = new IssueDetailModel()
            {
                Issue = issueFacade.GetIssueById(issueId),
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByIssue(issueId)
                }
            };
            return View("IssueDetail", model);
        }

        public ActionResult CreateIssue()
        {
            var model = new EditIssueModel()
            {
                Issue = new IssueDTO(),
                ExistingProjects = projectFacade.GetAllProjects(),
                ExistingPeople = personFacade.GetAllPeople(),
                ExistingEmployees = employeeFacade.GetAllEmployees()
            };
            return View("CreateIssue", model);
        }

        [HttpPost]
        public ActionResult CreateIssue(EditIssueModel model)
        {
            model.Issue.Created = DateTime.Now;
            issueFacade.CreateIssue(model.Issue, 
                model.SelectedProjectId,
                model.SelectedCreatorId, 
                model.SelectedEmployeeId);
            return RedirectToAction("ViewAllIssues");
        }

        public ActionResult EditIssue(int issueId)
        {
            var model = new EditIssueModel()
            {
                Issue = issueFacade.GetIssueById(issueId),
                ExistingProjects = projectFacade.GetAllProjects(),
                ExistingPeople = personFacade.GetAllPeople(),
                ExistingEmployees = employeeFacade.GetAllEmployees()
            };
            model.SelectedProjectId = model.Issue.Project.Id;
            model.SelectedEmployeeId = model.Issue.AssignedEmployee.Id;
            model.SelectedCreatorId = model.Issue.Creator.Id;
            return View("EditIssue", model);
        }

        [HttpPost]
        public ActionResult EditIssue(EditIssueModel model)
        {
            issueFacade.UpdateIssue(model.Issue,
                model.SelectedProjectId,
                model.SelectedCreatorId,
                model.SelectedEmployeeId);
            return RedirectToAction("ViewAllIssues");
        }

        public ActionResult DeleteIssue(int issueId)
        {
            var issue = issueFacade.GetIssueById(issueId);
            issueFacade.DeleteIssue(issue);
            return RedirectToAction("ViewAllIssues");
        }
    }
}