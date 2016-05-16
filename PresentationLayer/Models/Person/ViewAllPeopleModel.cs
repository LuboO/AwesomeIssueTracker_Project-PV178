using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Person
{
    public class ViewAllPeopleModel
    {
        public List<PersonDTO> People { get; set; }

        public ViewAllPeopleModel()
        {
            People = new List<PersonDTO>();
        }
    }
}