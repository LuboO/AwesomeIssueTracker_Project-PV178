namespace BussinesLayer.DTOs
{
    public class IssueFilter
    {
        public int? IssueId { get; set; }

        public int? ProjectId { get; set; }

        public int? AssignedEmployeeId { get; set; }

        public string Title { get; set; }
    }
}
