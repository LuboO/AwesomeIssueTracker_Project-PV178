using BussinesLayer.DTOs;
using PresentationLayer.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models.Customer
{
    public class CustomerDetailModel
    {
        public ListProjectsModel ListProjectsModel { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}