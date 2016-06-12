using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Project;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class ProjectController : Controller
    {
        private readonly ProjectFacade projectFacade;
        private readonly IssueFacade issueFacade;
        private readonly CustomerFacade customerFacade;

        public ProjectController(ProjectFacade projectFacade , IssueFacade issueFacade, CustomerFacade customerFacade)
        {
            this.projectFacade = projectFacade;
            this.issueFacade = issueFacade;
            this.customerFacade = customerFacade;
        }

        public ActionResult ViewAllProjects()
        {
            var model = new ViewAllProjectsModel()
            {
                ExistingCustomers = customerFacade.GetAllCustomers(),
                Projects = projectFacade.GetAllProjects()
                    .Select(p => new ProjectOverviewModel
                    {
                        ProjectId = p.Id,
                        ProjectName = p.Name,
                        CustomerId = p.Customer.Id,
                        CustomerName = p.Customer.User.Name,
                        ErrorCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Error).Count,
                        RequirementCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Requirement).Count
                    })
                    .ToList()
            };

            return View("ViewAllProjects", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewAllProjects(ViewAllProjectsModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View("ViewAllProjects");

            model.ExistingCustomers = customerFacade.GetAllCustomers();
            model.Projects = projectFacade.GetProjectsByCustomer(model.FilterByCustomerId)
                .Select(p => new ProjectOverviewModel()
                {
                    ProjectId = p.Id,
                    ProjectName = p.Name,
                    CustomerId = p.Customer.Id,
                    CustomerName = p.Customer.User.Name,
                    ErrorCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Error).Count,
                    RequirementCount = issueFacade.GetIssuesByProjectType(p.Id, IssueType.Requirement).Count
                })
                .ToList();

            return View("ViewAllProjects", model);
        }

        public ActionResult ProjectDetail(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);
            if(project == null)
                return View("BadInput");

            var model = new ProjectDetailModel()
            {
                Project = project,
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByProject(projectId.Value)
                },
                CanModify = User.IsInRole(UserRole.Administrator.ToString()) ||
                            User.Identity.GetUserId<int>() == project.Customer.Id,

        };
            return View("ProjectDetail", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectDetail(ProjectDetailModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View("BadView");

            var project = projectFacade.GetProjectById(model.Project.Id);
            if (project == null)
                return View("BadInput");

            var types = new List<IssueType>();
            var statuses = new List<IssueStatus>();

            if (model.ShowErrors)
                types.Add(IssueType.Error);
            if (model.ShowRequirements)
                types.Add(IssueType.Requirement);

            if (model.ShowNew)
                statuses.Add(IssueStatus.New);
            if (model.ShowAccepted)
                statuses.Add(IssueStatus.Accepted);
            if (model.ShowRejected)
                statuses.Add(IssueStatus.Rejected);
            if (model.ShowClosed)
                statuses.Add(IssueStatus.Closed);

            model.Project = project;
            model.ListIssuesModel = new ListIssuesModel()
            {
                Issues = issueFacade.GetIssuesByProjectTypeStatus(project.Id, types, statuses)
            };

            return View("ProjectDetail", model);
        }

        [CustomAuthorize(Roles = "Customer")]
        public ActionResult CreateProject()
        {
            var model = new EditProjectModel();
            return View("CreateProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Customer")]
        public ActionResult CreateProject(EditProjectModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            var project = new ProjectDTO()
            {
                Name = model.Name,
                Description = model.Description
            };
            var newId = projectFacade.CreateProject(project, User.Identity.GetUserId<int>());
            return RedirectToAction("ProjectDetail", new { projectId = newId });
        }

        [CustomAuthorize(Roles = "Administrator,Customer")]
        public ActionResult EditProject(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);
            if(project == null)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != project.Customer.Id))
                return View("AccessForbidden");

            var model = new EditProjectModel()
            {
                ProjectId = project.Id,
                CustomerId = project.Customer.Id,
                Name = project.Name,
                Description = project.Description
            };

            return View("EditProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator,Customer")]
        public ActionResult EditProject(EditProjectModel model)
        {
            if (model == null)
                return View("BadInput");

            if (!ModelState.IsValid)
                return View(model);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.CustomerId))
                return View("AccessForbidden");

            var project = projectFacade.GetProjectById(model.ProjectId);
            if(project == null)
                return View("BadInput");

            project.Id = model.ProjectId;
            project.Name = model.Name;
            project.Description = model.Description;

            projectFacade.UpdateProject(project);
            return RedirectToAction("ProjectDetail", new { projectId = model.ProjectId });
        }

        [CustomAuthorize(Roles = "Administrator,Customer")]
        public ActionResult DeleteProject(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);
            if(project == null)
                return View("BadInput");

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != project.Customer.Id))
                return View("AccessForbidden");

            projectFacade.DeleteProject(project);
            return RedirectToAction("ViewAllProjects");
        }
    }
}