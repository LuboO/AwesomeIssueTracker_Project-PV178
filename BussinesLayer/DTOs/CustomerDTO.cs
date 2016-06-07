using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required]
        public UserDTO User { get; set; }

        [Required]
        [Range(1,2)]
        [Display(Name = "Type of customer")]
        public CustomerType Type { get; set; }

        public override string ToString()
        {
            return $"Customer {Id}: {User.Name}";
        }
    }
}
