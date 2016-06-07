using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;

namespace PresentationLayer.Models.Issue
{
    public class IssueDetailModel
    {
        public IssueDTO Issue { get; set; }

        public string NewCommentSubject { get; set; }

        public string NewCommentMessage { get; set; }

        public ListCommentsModel ListCommentsModel { get; set; }

        public bool CanChangeState { get; set; }

        public bool CanModify { get; set; }
    }
}