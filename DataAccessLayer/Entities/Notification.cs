using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents notification, holds issue that is source of
    /// notification and person that should be notified. Email is sent
    /// if NotifyByEmail is set to true.
    /// </summary>
    public class Notification : IEntity<int>
    {
        /// <summary>
        /// Unique Id of the Notification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Required information, when set to true email will be sent to Person
        /// on Issue change
        /// </summary>
        [Required]
        public bool NotifyByEmail { get; set; }

        /// <summary>
        /// Required Id of linked Issue
        /// </summary>
        public int IssueId { get; set; }
        /// <summary>
        /// Navigation property to Issue
        /// </summary>
        [Required,ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }

        /// <summary>
        /// Required Id of linked Person
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Navigation property to Person
        /// </summary>
        [Required,ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public override string ToString()
        {
            return $"Notification {Id}: {Person.Name}, {Issue.Title}: {NotifyByEmail}";
        }
    }
}
