using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class IssueDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public IssueType Type { get; set; }

        [Required]
        public IssueStatus Status { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Finished { get; set; }

        [Required]
        public virtual ProjectDTO Project { get; set; }

        [Required]
        public virtual EmployeeDTO AssignedEmployee { get; set; }

        [Required]
        public virtual PersonDTO Creator { get; set; }
    
        [Required]
        public virtual List<NotificationDTO> Notifications { get; set; }

        public virtual List<CommentDTO> Comments { get; set; }

        public IssueDTO()
        {
            Notifications = new List<NotificationDTO>();
            Comments = new List<CommentDTO>();
        }

        public override string ToString()
        {
            return $"Issue {Id}: {Title}";
        }
    }
}
