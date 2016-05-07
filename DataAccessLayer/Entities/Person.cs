using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Person in the system, holds personal information, both Customer
    /// and Employee are derived from this table. Holds issues created,
    /// comments authored and notifications requested.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique Person Id acts as primary key in Customer and Employee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Required name of Person
        /// </summary>
        [Required,MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Required email of Person
        /// </summary>
        [Required,MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// Optional adress of Person
        /// </summary>
        [MaxLength(256)]
        public string Adress { get; set; }
        
        /// <summary>
        /// Optional phone number of Person
        /// </summary>
        [MaxLength(64)]
        public string Phone { get; set; }

        /// <summary>
        /// Optional date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Link to Customer, can be null
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Link to Employee, can be null
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Navgation property to linked Issues
        /// </summary>
        public virtual List<Issue> Issues { get; set; }

        /// <summary>
        /// Navigation property to linked Comments
        /// </summary>
        public virtual List<Comment> Comments { get; set; }

        /// <summary>
        /// Navigation property to linked Notifications
        /// </summary>
        public virtual List<Notification> Notifications { get; set; }

        public Person()
        {
            Issues = new List<Issue>();
            Comments = new List<Comment>();
            Notifications = new List<Notification>();
        }

        public override string ToString()
        {
            return $"Person {Id}: {Name}, {Email}";
        }
    }
}
