using BussinesLayer.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models.Issue
{
    public class EditIssueModel
    {
        public IssueDTO Issue { get; set; }

        public int SelectedCreatorId { get; set; }

        public int SelectedProjectId { get; set; }

        public int SelectedEmployeeId { get; set; }

        public List<ProjectDTO> ExistingProjects { get; set; }

        //public List<PersonDTO> ExistingPeople { get; set; }
        public List<UserDTO> ExistingUsers { get; set; }

        public List<EmployeeDTO> ExistingEmployees { get; set; }

        public IEnumerable<SelectListItem> ProjectItems
        {
            get
            {
                var rval = ExistingProjects
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    });
                return rval;
            }
        }

        public IEnumerable<SelectListItem> PeopleItems
        {
            get
            {
                var rval = ExistingUsers
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    });
                return rval;
            }
        }

        public IEnumerable<SelectListItem> EmployeeItems
        {
            get
            {
                var rval = ExistingEmployees
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.User.Name
                    });
                return rval;
            }
        }
    }
}