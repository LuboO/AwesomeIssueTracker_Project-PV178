using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Customer
{
    public class ViewAllCustomersModel
    {
        public List<CustomerDTO> Customers { get; set; }

        public ViewAllCustomersModel()
        {
            Customers = new List<CustomerDTO>();
        }
    }
}