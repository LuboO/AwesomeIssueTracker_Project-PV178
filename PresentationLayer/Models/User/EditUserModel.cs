using System;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.User
{
    public class EditUserModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string Name { get; set; }

        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfBirth { get; set; }
    }
}