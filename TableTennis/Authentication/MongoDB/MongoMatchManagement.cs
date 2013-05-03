using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.Authentication.MongoDB
{
    public class MongoMatchManagement : IMatchManagementRepository
    {
        private readonly string _connStr;
        private readonly MongoClient _mongoClient;
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;

        public MongoMatchManagement()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            _mongoClient = new MongoClient(_connStr);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase("tabletennis");
        }

        public void CreateMatch(PlayedGame game)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Insert(game);
        }

        public List<PlayedGame> GetAllGames()
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.FindAll().ToList();
        }

        public List<PlayedGame> GetAllGamesByPlayerID(Guid playerID)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.Find(Query<PlayedGame>.Where(g => g.PlayerIds.Contains(playerID))).ToList();
        }

        public void UpdateGameRatingById(PlayedGame game)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Save(game);
        }
    }
}