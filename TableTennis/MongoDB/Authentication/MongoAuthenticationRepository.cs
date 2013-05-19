using System.Linq;
using MongoDB.Driver.Builders;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.MongoDB.Authentication
{
    public class MongoAuthenticationRepository : MongoRepositoryBase, IAuthenticationRepository
    {
        public bool ValidateUser(string user, string password)
        {
            var collection = _mongoDatabase.GetCollection<UserAccount>("Users");

            var foundUser = collection.FindOne(Query<UserAccount>.Where(s => s.Username == user));

            if (foundUser != null)
            {
                var matched = BCrypt.Net.BCrypt.Verify(password, foundUser.Password);
                return matched;
            }
            return false;
        }

        public void CreateUser(string username, string password)
        {
            const int bcryptWorkFactor = 10;
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, bcryptWorkFactor);

            var UserAccount = new UserAccount { Username = username, Password = hashedPassword };

            var collection = _mongoDatabase.GetCollection<UserAccount>("Users");
            collection.Insert(UserAccount);
        }

        public string[] GetUserRoles(string username)
        {
            var collection = _mongoDatabase.GetCollection<UserAccount>("Users");
            var foundUser = collection.FindOne(Query<UserAccount>.Where(s => s.Username == username));

            return foundUser.Roles.Select(role => role.Name).ToArray();
        }

        public bool DoUserHaveRole(string username, string rolename)
        {
            var collection = _mongoDatabase.GetCollection<UserAccount>("Users");
            var foundUser = collection.FindOne(Query<UserAccount>.Where(s => s.Username == username));

            return foundUser.Roles.Exists(role => role.Name == rolename);
        }
    }
}