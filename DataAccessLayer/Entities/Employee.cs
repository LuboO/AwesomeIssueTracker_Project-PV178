using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents employee in the system, stores issues
    /// assigned to employee.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Key as well as foreign key to Person
        /// </summary>
        [Key,ForeignKey("Person")]
        public int PersonId { get; set; }
        /// <summary>
        /// Navigational property to Person
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// Navigational property to Issues that were assigned to this Employee
        /// </summary>
        public virtual List<Issue> AssignedIssues { get; set; }

        public Employee()
        {
            AssignedIssues = new List<Issue>();
        }

        public override string ToString()
        {
            return $"Employee {PersonId}: {Person.Name}";
        }
    }
}
