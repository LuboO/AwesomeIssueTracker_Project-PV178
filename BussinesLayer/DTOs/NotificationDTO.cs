using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        [Required]
        public bool NotifyByEmail { get; set; }

        [Required]
        public IssueDTO Issue { get; set; }

        [Required]
        public PersonDTO Person { get; set; }

        public override string ToString()
        {
            return $"Notification {Id}: {Person.Name}, {Issue.Title}: {NotifyByEmail}";
        }
    }
}
