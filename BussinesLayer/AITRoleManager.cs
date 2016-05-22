using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;

namespace BussinesLayer
{
    public class AITRoleManager : RoleManager<AITRole, int>
    {
        public AITRoleManager(IRoleStore<AITRole, int> store) : base(store)
        {
        }
    }
}
