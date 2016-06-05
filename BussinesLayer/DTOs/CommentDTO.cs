using System;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public IssueDTO Issue { get; set; }

        [Required]
        public UserDTO Author { get; set; }

        public override string ToString()
        {
            return $"Comment {Id}: {Author.Name}, {Issue.Title}: {Message}";
        }
    }
}
