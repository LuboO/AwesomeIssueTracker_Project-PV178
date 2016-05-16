using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Project;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
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

        public ActionResult ProjectDetail(int projectId)
        {
            var model = new ProjectDetailModel()
            {
                Project = projectFacade.GetProjectById(projectId),
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByProject(projectId)
                }
            };
            return View("ProjectDetail", model);
        }

        public ActionResult CreateProject()
        {
            var model = new EditProjectModel()
            {
                Project = new ProjectDTO(),
                ExistingCustomers = customerFacade.GetAllCustomers()
            };
            return View("CreateProject", model);
        }

        [HttpPost]
        public ActionResult CreateProject(EditProjectModel model)
        {
            projectFacade.CreateProject(model.Project, model.SelectedCustomerId);
            return RedirectToAction("ViewAllProjects");
        }

        public ActionResult EditProject(int projectId)
        {
            var model = new EditProjectModel
            {
                Project = projectFacade.GetProjectById(projectId),
                ExistingCustomers = customerFacade.GetAllCustomers()
            };
            model.SelectedCustomerId = model.Project.Customer.Id;
            return View("EditProject", model);
        }

        [HttpPost]
        public ActionResult EditProject(EditProjectModel editProjectModel)
        {
            projectFacade.UpdateProject(editProjectModel.Project , editProjectModel.SelectedCustomerId);
            return RedirectToAction("ViewAllProjects");
        }

        public ActionResult DeleteProject(int projectId)
        {
            var project = projectFacade.GetProjectById(projectId);
            projectFacade.DeleteProject(project);
            return RedirectToAction("ViewAllProjects");
        }
    }
}