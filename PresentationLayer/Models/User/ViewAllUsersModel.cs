using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.User
{
    public class ViewAllUsersModel
    {
        public List<UserDTO> Users { get; set; }

        public ViewAllUsersModel()
        {
            Users = new List<UserDTO>();
        }
    }
}