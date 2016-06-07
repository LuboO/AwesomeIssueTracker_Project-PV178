using System.ComponentModel.DataAnnotations;

namespace BussinesLayer.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Notify by E-Mail")]
        public bool NotifyByEmail { get; set; }

        [Required]
        public IssueDTO Issue { get; set; }

        [Required]
        public UserDTO User { get; set; }

        public override string ToString()
        {
            return $"Notification {Id}: {User.Name}, {Issue.Title}: {NotifyByEmail}";
        }
    }
}
