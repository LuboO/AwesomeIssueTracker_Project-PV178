using BussinesLayer.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models.Project
{
    public class EditProjectModel
    {
        public ProjectDTO Project { get; set; }

        public int SelectedCustomerId { get; set; }

        public List<CustomerDTO> ExistingCustomers { get; set; }

        public IEnumerable<SelectListItem> CustomerItems
        {
            get
            {
                var rval = ExistingCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Person.Name
                    });
                return rval;
            }
        }

        public EditProjectModel()
        {
            ExistingCustomers = new List<CustomerDTO>();
        }
    }
}