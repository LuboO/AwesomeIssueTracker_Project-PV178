using DataAccessLayer.Enums;

namespace BussinesLayer.Filters
{
    public class IssueFilter
    {
        public int? IssueId { get; set; }

        public int? ProjectId { get; set; }

        public int? CreatorId { get; set; }

        public int? AssignedEmployeeId { get; set; }

        public string Title { get; set; }

        public IssueType? Type { get; set; }

        public IssueStatus? Status { get; set; }
    }
}
