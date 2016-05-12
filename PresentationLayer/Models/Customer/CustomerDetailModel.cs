using BussinesLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models.Customer
{
    public class CustomerDetailModel
    {
        public CustomerDTO Customer { get; set; }

        public List<ProjectDTO> Projects { get; set; }

        public CustomerDetailModel()
        {
            Projects = new List<ProjectDTO>();
        }
    }
}