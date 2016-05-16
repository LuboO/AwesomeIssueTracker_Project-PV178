using System;
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

        public override string ToString()
        {
            return $"Person {Id}: {Name}, {Email}";
        }
    }
}
