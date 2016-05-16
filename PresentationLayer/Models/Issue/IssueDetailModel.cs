using BussinesLayer.DTOs;
using PresentationLayer.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models.Issue
{
    public class IssueDetailModel
    {
        public IssueDTO Issue { get; set; }
        public ListCommentsModel ListCommentsModel { get; set; }
    }
}