using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;

namespace PresentationLayer.Models.Issue
{
    public class IssueDetailModel
    {
        public IssueDTO Issue { get; set; }
        public ListCommentsModel ListCommentsModel { get; set; }
    }
}