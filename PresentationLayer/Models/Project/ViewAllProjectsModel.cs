using BussinesLayer.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models.Project
{
    public class ViewAllProjectsModel
    {
        [Required]
        public int FilterByCustomerId { get; set; }
        
        public List<CustomerDTO> ExistingCustomers { get; set; }
        
        public IEnumerable<SelectListItem> CustomerItems
        {
            get
            {
                var rval = ExistingCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.User.Name + " (" + c.User.UserName + ")"
                    });
                return rval;
            }
        }

        public List<ProjectOverviewModel> Projects;
    }
}