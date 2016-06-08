using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Comment
{
    public class PostCommentModel
    {
        public int IssueId { get; set; }

        [MaxLength(256)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(4096)]
        public string Message { get; set; }
    }
}