using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Person
{
    public class ListPeopleModel
    {
        public List<PersonDTO> People { get; set; }

        public ListPeopleModel()
        {
            People = new List<PersonDTO>();
        }
    }
}