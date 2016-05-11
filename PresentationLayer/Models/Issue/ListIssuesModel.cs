using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Issue
{
    public class ListIssuesModel
    {
        public List<IssueDTO> Issues { get; set; }

        public ListIssuesModel()
        {
            Issues = new List<IssueDTO>();
        }
    }
}