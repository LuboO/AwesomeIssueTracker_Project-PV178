using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents employee in the system, stores issues
    /// assigned to employee.
    /// </summary>
    public class Employee : IEntity<int>
    {
        /// <summary>
        /// Key as well as foreign key to Person
        /// </summary>
        [ForeignKey("Person")]
        public int Id { get; set; }
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
            return $"Employee {Id}: {Person.Name}";
        }
    }
}
