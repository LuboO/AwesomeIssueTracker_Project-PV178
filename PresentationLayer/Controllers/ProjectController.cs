using BussinesLayer.Facades;
using PresentationLayer.Models.Project;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectFacade projectFacade;

        public ProjectController(ProjectFacade projectFacade)
        {
            this.projectFacade = projectFacade;
        }

        public ActionResult ListProjects()
        {
            var listProjectsModel = new ListProjectsModel()
            {
                Projects = projectFacade.GetAllProjects()
            };
            return View("ListProjects", listProjectsModel);
        }
    }
}