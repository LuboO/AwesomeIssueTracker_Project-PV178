using BussinesLayer.DTOs;
using PresentationLayer.Models.Project;
using System.Collections.Generic;

namespace PresentationLayer.Models.Customer
{
    public class CustomerDetailModel
    {
        public List<ProjectOverviewModel> Projects { get; set; }

        public CustomerDTO Customer { get; set; }
    }
}