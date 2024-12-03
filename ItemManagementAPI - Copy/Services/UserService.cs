using ItemManagementAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
namespace ItemManagementAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UsersCollectionName); 
        }

        
        public User Authenticate(string email, string password)
        {
            var user = GetByEmail(email); ;
            if (user == null || user.Password != HashPassword(password) )
                return null;

            return user;
        }

        public User GetByEmail(string email)
        {
            return _users.Find(user => user.Email == email).FirstOrDefault();
        }
        //hash function
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public void Register(User userDto)
        {
            userDto.Role ??= "Admin"; //13.11.14
            // Hash the password 
            userDto.Password = HashPassword(userDto.Password);

            // Insert the user into the Users collection
            _users.InsertOne(userDto);
        }

    }
}