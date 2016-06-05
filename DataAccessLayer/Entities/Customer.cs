using DataAccessLayer.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents customer in the system, holds information
    /// about projects created by customer and type of of customer.
    /// Type can be NaturalPerson or LegalEntity.
    /// </summary>
    public class Customer : IEntity<int>
    {
        /// <summary>
        /// Key as well as foreign key to Person
        /// </summary>
        [ForeignKey("User")]
        public int Id { get; set; }

        /// <summary>
        /// Navigational property to linked Person
        /// </summary>
        public virtual AITUser User { get; set; }

        /// <summary>
        /// Mandatory Type of the Customer, can be either NaturalPerson or LegalEntity
        /// </summary>
        [Required,Range(1,2)]
        public CustomerType Type { get; set; }

        /// <summary>
        /// Navigational property to Customer's Projects
        /// </summary>
        public virtual List<Project> Projects { get; set; }

        public Customer()
        {
            Projects = new List<Project>();
        }

        public override string ToString()
        {
            return $"Customer {Id}: {User.Name}";
        }
    }
}
