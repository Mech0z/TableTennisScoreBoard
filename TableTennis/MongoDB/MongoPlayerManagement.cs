using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.MongoDB
{
    public class MongoPlayerManagement : MongoRepositoryBase, IPlayerManagementRepository
    {
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

        public Player GetPlayerByUsername(string username)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.Find(Query<Player>.Where(s => s.Username == username)).Single();
        }

        public int GetPlayerRatingByUsername(string username)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            return collection.Find(Query<Player>.Where(s => s.Username == username)).Select(p => p.Rating).Single();
        }

        public void UpdateRating(string username, int rating)
        {
            var collection = _mongoDatabase.GetCollection<Player>("Player");
            var player = collection.Find(Query<Player>.Where(s => s.Username == username)).Single();
            player.Rating = rating;
            collection.Save(player);
        }

        //public List<PlayerUsername> GetPlayerUsernames(List<Guid> playerIds)
        //{
        //    var collection = _mongoDatabase.GetCollection<Player>("Player");
        //    var list = new List<PlayerUsername>();

        //    foreach (var playerId in playerIds)
        //    {
        //        var player = collection.FindOne(Query<Player>.Where(p => p.Id == playerId));
        //        list.Add(new PlayerUsername { PlayerID = playerId, PlayerUserName = player.Username });
        //    }

        //    return list;
        //}
    }
}