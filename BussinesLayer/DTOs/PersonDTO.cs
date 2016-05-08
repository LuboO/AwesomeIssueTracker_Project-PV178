using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class PersonDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Adress { get; set; }

        public string Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public virtual CustomerDTO Customer { get; set; }

        public virtual EmployeeDTO Employee { get; set; }

        public virtual List<IssueDTO> Issues { get; set; }

        public virtual List<CommentDTO> Comments { get; set; }

        public virtual List<NotificationDTO> Notifications { get; set; }

        public PersonDTO()
        {
            Issues = new List<IssueDTO>();
            Comments = new List<CommentDTO>();
            Notifications = new List<NotificationDTO>();
        }

        public override string ToString()
        {
            return $"Person {Id}: {Name}, {Email}";
        }
    }
}
