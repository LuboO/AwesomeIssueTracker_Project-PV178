using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents project, is linked to its Customer and has 
    /// associated Issues. Also holds name and description.
    /// </summary>
    public class Project : IEntity<int>
    {
        /// <summary>
        /// Unique Project Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mandatory name of the Project
        /// </summary>
        [Required,MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Optional description of the project
        /// </summary>
        [MaxLength(4096)]
        public string Description { get; set; }

        /// <summary>
        /// Mandatory Id of linked Customer
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Navigation property to Customer
        /// </summary>
        [Required,ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Navigation property by Project's Issues
        /// </summary>
        public virtual List<Issue> Issues { get; set; }

        public Project()
        {
            Issues = new List<Issue>();
        }

        public override string ToString()
        {
            return $"Project {Id}: {Name}";
        }
    }
}
