using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required]
        public PersonDTO Person { get; set; }

        public CustomerType Type { get; set; }

        public override string ToString()
        {
            return $"Customer {Id}: {Person.Name}";
        }
    }
}
