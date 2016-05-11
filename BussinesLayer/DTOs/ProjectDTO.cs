using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public CustomerDTO Customer { get; set; }

        public override string ToString()
        {
            return $"Project {Id}: {Name}";
        }
    }
}
