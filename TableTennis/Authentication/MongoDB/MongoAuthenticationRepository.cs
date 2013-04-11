using System.Linq;
using GoldSilverWebServer.Authentication.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TableTennis.Models;

namespace TableTennis.Authentication.MongoDB
{
    public class MongoAuthenticationRepository : IMongoAuthenticationRepository
    {
        private readonly string _connStr;
        private readonly MongoClient _mongoClient;
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;

        public MongoAuthenticationRepository()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;

            _mongoClient = new MongoClient(_connStr);

            _mongoServer = _mongoClient.GetServer();

            _mongoDatabase = _mongoServer.GetDatabase("goldsilverprofit");
        }

        public bool ValidateUser(string user, string password)
        {
            var collection = _mongoDatabase.GetCollection<UserAccount>("Users");

            var foundUser = collection.FindOne(Query<UserAccount>.Where(s => s.Username == user));

            var matched = BCrypt.Net.BCrypt.Verify(password, foundUser.Password);

            return foundUser != null && matched;
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