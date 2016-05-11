using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Project
{
    public class ListProjectsModel
    {
        public List<ProjectDTO> Projects { get; set; }

        public ListProjectsModel()
        {
            Projects = new List<ProjectDTO>();
        }
    }
}