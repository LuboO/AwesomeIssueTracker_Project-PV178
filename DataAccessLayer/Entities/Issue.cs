﻿using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Represents issue in the system. Holds information about issue
    /// author, project and assigned employee. Moreover it holds comments
    /// and notifications linked to issue and title, description, time of creation
    /// and closure, status and type of issue.
    /// </summary>
    public class Issue : IEntity<int>
    {
        /// <summary>
        /// Unique Id of the issue
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mandatory title of the Issue
        /// </summary>
        [Required, MaxLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// Optional description of the Issue
        /// </summary>
        [MaxLength(4096)]
        public string Description { get; set; }

        /// <summary>
        /// Mandatory type of the Issue, can be either Error or Requirement
        /// </summary>
        [Required,Range(1,2)]
        public IssueType Type { get; set; }

        /// <summary>
        /// Mandatory status of the Issue, can be one of New, Accepted, Rejected, Closed
        /// </summary>
        [Required,Range(1,4)]
        public IssueStatus Status { get; set; }

        /// <summary>
        /// Mandatory datetime of Issu creation
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Datetime of Issue closure, can be empty
        /// </summary>
        public DateTime? Finished { get; set; }

        /// <summary>
        /// Time of last change of Issue. Will be null for new Issue.
        /// </summary>
        public DateTime? ChangeTime { get; set; }

        /// <summary>
        /// Type of last Issue change. E.g. Updated, Comment, Reject, etc...
        /// </summary>
        [Range(1,6)]
        public IssueChangeType? ChangeType { get; set; }

        /// <summary>
        /// Name of User who last changed this Issue.
        /// </summary>
        public string NameOfChanger { get; set; }

        /// <summary>
        /// Mandatory Id of the Project this issue is created in
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
        /// <summary>
        /// Navigational property to linked project
        /// </summary>
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        /// <summary>
        /// Mandatory Id of Employee assigned to Issue
        /// </summary>
        [Required]
        public int AssignedEmployeeId { get; set; }
        /// <summary>
        /// Navigational property to linked Employee
        /// </summary>
        [ForeignKey("AssignedEmployeeId")]
        public virtual Employee AssignedEmployee { get; set; }

        /// <summary>
        /// Mandatory Id of creator of Issue
        /// </summary>
        [Required]
        public int CreatorId { get; set; }
        /// <summary>
        /// Navigation property to linked Person
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual AITUser Creator { get; set; }

        /// <summary>
        /// Navigation property to linked Notifications
        /// </summary>
        public virtual List<Notification> Notifications { get; set; }

        /// <summary>
        /// Navigation property to linked Comments
        /// </summary>
        public virtual List<Comment> Comments { get; set; }

        public Issue()
        {
            Notifications = new List<Notification>();
            Comments = new List<Comment>();
        }

        public override string ToString()
        {
            return $"Issue {Id}: {Title}";
        }
    }
}
