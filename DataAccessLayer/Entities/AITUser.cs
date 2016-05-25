using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class AITUser : IdentityUser<int, AITUserLogin, AITUserRole, AITUserClaim>
    {
        /// <summary>
        /// Required name of Person
        /// </summary>
        [Required, MaxLength(64)]
        public override string UserName { get; set; }

        /// <summary>
        /// Required email of Person
        /// </summary>
        [Required, MaxLength(64), Index(IsUnique = true)]
        public override string Email { get; set; }

        /// <summary>
        /// Optional adress of Person
        /// </summary>
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// Optional phone number of Person
        /// </summary>
        [MaxLength(64)]
        public override string PhoneNumber { get; set; }

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

        public AITUser()
        {
            Issues = new List<Issue>();
            Comments = new List<Comment>();
            Notifications = new List<Notification>();
        }

        public override string ToString()
        {
            return $"User {Id}: {UserName}, {Email}";
        }
    }
}
