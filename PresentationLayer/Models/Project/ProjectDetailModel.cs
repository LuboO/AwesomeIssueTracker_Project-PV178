using BussinesLayer.DTOs;
using PresentationLayer.Models.Issue;

namespace PresentationLayer.Models.Project
{
    public class ProjectDetailModel
    {
        public ListIssuesModel ListIssuesModel { get; set; }

        public ProjectDTO Project { get; set; }

        public bool CanModify { get; set; }
    }
}