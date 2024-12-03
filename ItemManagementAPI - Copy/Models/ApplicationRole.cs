using AspNetCore.Identity.MongoDbCore.Models;

namespace ItemManagementAPI.Models
{
    public class ApplicationRole : MongoDbIdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string role) : base(role) {}
    }
}
