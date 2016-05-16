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

        public ProjectController(ProjectFacade projectFacade , IssueFacade issueFacade)
        {
            this.projectFacade = projectFacade;
            this.issueFacade = issueFacade;
        }

        public ActionResult ViewAllProjects()
        {
            var viewAllProjectsModel = new ViewAllProjectsModel()
            {
                ListProjectsModel = new ListProjectsModel()
                {
                    Projects = projectFacade.GetAllProjects()
                }
            };
            return View("ViewAllProjects", viewAllProjectsModel);
        }

        public ActionResult ProjectDetail(int projectId)
        {
            var projectDetailModel = new ProjectDetailModel()
            {
                Project = projectFacade.GetProjectById(projectId),
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByProject(projectId)
                }
            };
            return View("ProjectDetail", projectDetailModel);
        }
    }
}