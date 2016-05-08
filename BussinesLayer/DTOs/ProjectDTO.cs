using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    class ProjectDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual CustomerDTO Customer { get; set; }

        public virtual List<IssueDTO> Issues { get; set; }

        public ProjectDTO()
        {
            Issues = new List<IssueDTO>();
        }

        public override string ToString()
        {
            return $"Project {Id}: {Name}";
        }
    }
}
