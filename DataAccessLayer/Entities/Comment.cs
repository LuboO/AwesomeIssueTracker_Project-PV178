using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents comment in the system, holds information about
    /// author, issue, subject and message of comment and time of 
    /// creation.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Unique Id of the comment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Optional subject of the comment
        /// </summary>
        [MaxLength(256)]
        public string Subject { get; set; }

        /// <summary>
        /// Mandatory message
        /// </summary>
        [Required,MaxLength(4096)]
        public string Message { get; set; }

        /// <summary>
        /// Datetime of comment creation
        /// </summary>
        [Required]
        public DateTime Created { get; set; }
        
        /// <summary>
        /// Mandatory Id of issue linked to comment
        /// </summary>
        public int IssueId { get; set; }
        /// <summary>
        /// Navigation property to commented issue
        /// </summary>
        [Required,ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }
       
        /// <summary>
        /// Mandatory Id of the author of the comment
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Navigation property to comment author
        /// </summary>
        [Required,ForeignKey("AuthorId")]
        public virtual Person Author { get; set; }

        public override string ToString()
        {
            return $"Comment {Id}: {Author.Name}, {Issue.Title}: {Message}";
        }
    }
}
