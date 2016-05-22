using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    public class AITUserRole : IdentityUserRole<int>
    {
        public string Code { get; set; }
    }
}
