using System;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.User
{
    public class EditAccountModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string Name { get; set; }

        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfBirth { get; set; }
    }
}