using Microsoft.AspNet.Identity.EntityFramework;

namespace PresentationLayer.Models.Identity
{
    public class ApplicationWebDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationWebDbContext()
            : base ("DefaultConnection", false)
        {
        }

        public static ApplicationWebDbContext Create()
        {
            return new ApplicationWebDbContext();
        }
    }
}