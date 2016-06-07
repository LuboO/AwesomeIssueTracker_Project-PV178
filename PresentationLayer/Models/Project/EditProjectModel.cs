using BussinesLayer.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models.Project
{
    public class EditProjectModel
    {
        public int ProjectId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(4096)]
        public string Description { get; set; }
    }
}