using MongoDB.Driver;

namespace TableTennis.MongoDB
{
    public class MongoRepositoryBase
    {
        private string _connStr;
        private MongoClient _mongoClient;
        private MongoServer _mongoServer;
        protected MongoDatabase _mongoDatabase;

        public MongoRepositoryBase()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            _mongoClient = new MongoClient(_connStr);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]);
        }
    }
}