using BussinesLayer.DTOs;
using PresentationLayer.Models.Issue;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Project
{
    public class ProjectDetailModel
    {
        public ListIssuesModel ListIssuesModel { get; set; }
        
        public ProjectDTO Project { get; set; }

        public bool CanModify { get; set; }

        public bool ShowErrors { get; set; }

        public bool ShowRequirements { get; set; }

        public bool ShowNew { get; set; }

        public bool ShowAccepted { get; set; }

        public bool ShowRejected { get; set; }

        public bool ShowClosed { get; set; }
    }
}