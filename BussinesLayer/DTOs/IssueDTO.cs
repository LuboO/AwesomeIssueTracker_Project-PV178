using DataAccessLayer.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class IssueDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Range(1, 2)]
        [Display(Name = "Type of issue")]
        public IssueType Type { get; set; }

        [Range(1, 4)]
        [Display(Name = "State of issue")]
        public IssueStatus Status { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy DD.mm.ss}")]
        [Display(Name = "Created on")]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy DD.mm.ss}")]
        [Display(Name = "Finished on")]
        public DateTime? Finished { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy DD.mm.ss}")]
        [Display(Name = "Last change")]
        public DateTime? ChangeTime { get; set; }
        
        [Range(1,6)]
        public IssueChangeType? ChangeType { get; set; }

        public string NameOfChanger { get; set; }

        public ProjectDTO Project { get; set; }

        [Display(Name = "Assigned employee")]
        public EmployeeDTO AssignedEmployee { get; set; }

        [Display(Name = "Issued by")]
        public UserDTO Creator { get; set; }

        public override string ToString()
        {
            return $"Issue {Id}: {Title}";
        }
    }
}
