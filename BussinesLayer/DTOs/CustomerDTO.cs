using DataAccessLayer.Enums;
using System.Collections.Generic;

namespace BussinesLayer.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public virtual PersonDTO Person { get; set; }

        public CustomerType Type { get; set; }

        public virtual List<ProjectDTO> Projects { get; set; }

        public CustomerDTO()
        {
            Projects = new List<ProjectDTO>();
        }

        public override string ToString()
        {
            return $"Customer {Id}: {Person.Name}";
        }
    }
}
