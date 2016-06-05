using DataAccessLayer.Enums;
using System;
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
        public ProjectDTO Project { get; set; }

        [Required]
        public EmployeeDTO AssignedEmployee { get; set; }

        [Required]
        public UserDTO Creator { get; set; }

        public override string ToString()
        {
            return $"Issue {Id}: {Title}";
        }
    }
}
