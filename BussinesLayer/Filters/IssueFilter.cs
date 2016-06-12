using DataAccessLayer.Enums;
using System.Collections.Generic;

namespace BussinesLayer.Filters
{
    public class IssueFilter
    {
        public int? IssueId { get; set; }

        public int? ProjectId { get; set; }

        public int? CreatorId { get; set; }

        public int? AssignedEmployeeId { get; set; }

        public string Title { get; set; }

        public List<IssueType> Types { get; set; }

        public List<IssueStatus> Statuses { get; set; }

        public IssueFilter()
        {
            Types = new List<IssueType>();
            Statuses = new List<IssueStatus>();
        }
    }
}
