namespace PresentationLayer.Models.Project
{
    public class ProjectOverviewModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
        
        public int ErrorCount { get; set; }

        public int RequirementCount { get; set; }
    }
}