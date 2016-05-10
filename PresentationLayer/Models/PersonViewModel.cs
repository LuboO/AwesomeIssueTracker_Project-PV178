using BussinesLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class PersonViewModel
    {
        public List<PersonDTO> People { get; set; }

        public PersonViewModel()
        {
            People = new List<PersonDTO>();
        }
    }
}