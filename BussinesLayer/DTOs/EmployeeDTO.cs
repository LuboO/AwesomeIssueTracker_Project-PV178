using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        [Required]
        public virtual PersonDTO Person { get; set; }

        public virtual List<IssueDTO> AssignedIssues { get; set; }

        public EmployeeDTO()
        {
            AssignedIssues = new List<IssueDTO>();
        }

        public override string ToString()
        {
            return $"Employee {Id}: {Person.Name}";
        }
    }
}
