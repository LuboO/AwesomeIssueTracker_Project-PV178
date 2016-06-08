using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Issue
{
    public class IssueDetailModel
    {
        public IssueDTO Issue { get; set; }

        [MaxLength(256)]
        public string NewCommentSubject { get; set; }

        [Required]
        [MaxLength(4096)]
        public string NewCommentMessage { get; set; }

        public ListCommentsModel ListCommentsModel { get; set; }

        public bool CanChangeState { get; set; }

        public bool CanModify { get; set; }
    }
}