using System.Configuration;
using MongoDB.Driver;

namespace TableTennis.Authentication.MongoDB
{
    public class MongoDBSettings
    {
        public void Connection()
        {
            var conn = ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;

            var client = new MongoClient(conn);


            var server = client.GetServer();
            

            var database = server.GetDatabase("goldsilverprofit");
            
            database.CreateCollection("Users");
            var collection = database.GetCollection("Users");

            var data = collection.FindAll();
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}