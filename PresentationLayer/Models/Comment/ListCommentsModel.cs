using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Comment
{
    public class ListCommentsModel
    {
        public List<CommentDTO> Comments { get; set; }

        public ListCommentsModel()
        {
            Comments = new List<CommentDTO>();
        }
    }
}