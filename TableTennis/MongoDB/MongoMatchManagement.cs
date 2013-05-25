using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.MongoDB
{
    public class MongoMatchManagement : MongoRepositoryBase, IMatchManagementRepository
    {
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

        public List<PlayedGame> GetAllGamesByUsername(string username)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.Find(Query<PlayedGame>.Where(g => g.Players.Contains(username))).ToList();
        }

        public void UpdateGameRatingById(PlayedGame game)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Save(game);
        }

        public List<PlayedGame> GetLastXPlayedGames(int numberOfGames, string boundAccount)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return
                collection.Find(Query<PlayedGame>.Where(game => game.BoundAccount == boundAccount)).OrderByDescending(
                    s => s.TimeStamp).Take(numberOfGames).ToList();
        }

        public void UpdateMatch(PlayedGame game)
        {
            var collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Save(game);
        }
    }
}