using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Issue
{
    public class IssueDetailModel
    {
        public IssueDTO Issue { get; set; }

        public ListCommentsModel ListCommentsModel { get; set; }

        public bool IsUserSubscribed { get; set; }

        public bool CanChangeState { get; set; }

        public bool CanModify { get; set; }
    }
}