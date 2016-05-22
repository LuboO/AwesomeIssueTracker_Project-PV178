using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    public class AITUser : IdentityUser<int, AITUserLogin, AITUserRole, AITUserClaim>
    {
    }
}
