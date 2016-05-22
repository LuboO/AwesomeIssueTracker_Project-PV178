using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;

namespace BussinesLayer
{
    public class AITUserManager : UserManager<AITUser, int>
    {
        public AITUserManager(IUserStore<AITUser, int> store) : base(store)
        {
        }
    }
}
