using BussinesLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models.Customer
{
    public class ListCustomersModel
    {
        public List<CustomerDTO> Customers { get; set; }

        public ListCustomersModel()
        {
            Customers = new List<CustomerDTO>();
        }
    }
}