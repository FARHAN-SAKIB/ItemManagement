namespace ItemManagementAPI.Models
{
    public class User
    {
        public string _id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Store hashed passwords in production
        public string Role { get; set; } //13.11.24
        public User()
        {
            _id = Guid.NewGuid().ToString();
            Role = "Admin";
        }
    }
}
