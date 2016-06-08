using BussinesLayer.DTOs;
using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Customer
{
    public class EditCustomerModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 2)]
        [Display(Name = "Type of customer")]
        public CustomerType Type { get; set; }
    }
}