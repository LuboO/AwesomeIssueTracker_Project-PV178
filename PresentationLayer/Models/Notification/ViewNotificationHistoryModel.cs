using BussinesLayer.DTOs;
using System.Collections.Generic;

namespace PresentationLayer.Models.Notification
{
    public class ViewNotificationHistoryModel
    {
        public List<IssueDTO> ChangedIssues { get; set; }

        public ViewNotificationHistoryModel()
        {
            ChangedIssues = new List<IssueDTO>();
        }
    }
}