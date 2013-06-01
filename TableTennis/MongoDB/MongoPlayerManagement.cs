using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TableTennis.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.MongoDB
{
    public class MongoPlayerManagement : MongoRepositoryBase, IPlayerManagementRepository
    {
        public bool CreatePlayer(Player player)
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            Player foundPlayer = collection.FindOne(Query<Player>.Where(s => s.Username == player.Username));

            if (foundPlayer != null)
            {
                return false;
            }

            collection.Insert(player);
            return true;
        }

        public List<Player> GetAllPlayers()
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.FindAll().ToList();
        }

        public Player GetPlayerByUsername(string username)
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.Find(Query<Player>.Where(s => s.Username == username)).Single();
        }

        public int GetPlayerRatingByUsername(string username)
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            return
                collection.Find(Query<Player>.Where(s => s.Username == username))
                          .Select(p => p.Ratings[Game.SingleTableTennis])
                          .Single();
        }

        public void UpdateRating(string username, int rating)
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            Player player = collection.Find(Query<Player>.Where(s => s.Username == username)).Single();
            player.Ratings[Game.SingleTableTennis] = rating;
            collection.Save(player);
        }

        public void UpdatePlayer(Player player)
        {
            MongoCollection<Player> collection = _mongoDatabase.GetCollection<Player>("Player");
            collection.Save(player);
        }
    }
}