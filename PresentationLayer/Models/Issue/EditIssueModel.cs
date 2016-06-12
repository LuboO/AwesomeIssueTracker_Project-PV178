using BussinesLayer.DTOs;
using DataAccessLayer.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models.Issue
{
    public class EditIssueModel
    {
        public int IssueId { get; set; }
        
        public int CreatorId { get; set; }
        
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(4096)]
        public string Description { get; set; }

        [Required]
        [Range(1, 2)]
        public IssueType Type { get; set; }

        [Required]
        public int SelectedEmployeeId { get; set; }

        public List<EmployeeDTO> ExistingEmployees { get; set; }

        public IEnumerable<SelectListItem> EmployeeItems
        {
            get
            {
                var rval = ExistingEmployees
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.User.Name + " (" + e.User.UserName + ")"
                    });
                return rval;
            }
        }
    }
}