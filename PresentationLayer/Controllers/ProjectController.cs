using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Project;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
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
                ListProjectsModel = new ListProjectsModel()
                {
                    Projects = projectFacade.GetAllProjects()
                }
            };
            return View("ViewAllProjects", model);
        }

        public ActionResult ProjectDetail(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);
            var model = new ProjectDetailModel()
            {
                Project = project,
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByProject(projectId.Value)
                }
            };
            model.CanModify = User.IsInRole(UserRole.Administrator.ToString()) ||
                User.Identity.GetUserId<int>() == project.Customer.Id;
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
            var project = new ProjectDTO()
            {
                Name = model.Name,
                Description = model.Description
            };
            var newId = projectFacade.CreateProject(project, User.Identity.GetUserId<int>());
            return RedirectToAction("ProjectDetail", new { projectId = newId });
        }

        public ActionResult EditProject(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);

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
        public ActionResult EditProject(EditProjectModel model)
        {
            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != model.CustomerId))
                return View("AccessForbidden");

            var project = new ProjectDTO()
            {
                Id = model.ProjectId,
                Name = model.Name,
                Description = model.Description
            };
            projectFacade.UpdateProject(project, model.CustomerId);
            return RedirectToAction("ProjectDetail", new { projectId = model.ProjectId });
        }

        public ActionResult DeleteProject(int? projectId)
        {
            if (!projectId.HasValue)
                return View("BadInput");

            var project = projectFacade.GetProjectById(projectId.Value);

            if (!User.IsInRole(UserRole.Administrator.ToString()) && (User.Identity.GetUserId<int>() != project.Customer.Id))
                return View("AccessForbidden");

            projectFacade.DeleteProject(project);
            return RedirectToAction("ViewAllProjects");
        }
    }
}