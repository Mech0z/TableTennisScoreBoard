using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;
using TableTennis.Models.Views.PlayerManagement;

namespace TableTennis.Authentication.MongoDB
{
    public class MongoPlayerManagement : IPlayerManagementRepository
    {
        private readonly string _connStr;
        private readonly MongoClient _mongoClient;
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;

        public MongoPlayerManagement()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            _mongoClient = new MongoClient(_connStr);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase("tabletennis");
        }

        public bool CreatePlayer(Player player)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            var foundPlayer = collection.FindOne(Query<Player>.Where(s => s.Username == player.Username));

            if (foundPlayer != null)
            {
                return false;
            }

            collection.Insert(player);
            return true;
        }

        public List<Player> GetAllPlayers()
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.FindAll().ToList();
        }

        public Player GetPlayerById(Guid playerId)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.Find(Query<Player>.Where(s => s.Id == playerId)).Single();
        }

        public int GetPlayerRatingById(Guid playerId)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.Find(Query<Player>.Where(s => s.Id == playerId)).Select(p => p.Rating).Single();
        }

        public void UpdateRating(Guid playerId, int rating)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            var player = collection.Find(Query<Player>.Where(s => s.Id == playerId)).Single();
            player.Rating = rating;
            collection.Save(player);
        }

        public List<PlayerUsername> GetPlayerUsernames(List<Guid> playerIds)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            var list = new List<PlayerUsername>();

            foreach (var playerId in playerIds)
            {
                var player = collection.FindOne(Query<Player>.Where(p => p.Id == playerId));
                list.Add(new PlayerUsername {PlayerID = playerId, PlayerUserName = player.Username});
            }

            return list;
        }
    }
}