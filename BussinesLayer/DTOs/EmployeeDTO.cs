using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        [Required]
        public UserDTO User { get; set; }

        public override string ToString()
        {
            return $"Employee {Id}: {User.Name}";
        }
    }
}
