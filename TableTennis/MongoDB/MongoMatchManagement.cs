using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TableTennis.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.MongoDB
{
    public class MongoMatchManagement : MongoRepositoryBase, IMatchManagementRepository
    {
        public void CreateMatch(PlayedGame game)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Insert(game);
        }

        public List<PlayedGame> GetAllGames()
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.FindAll().ToList();
        }

        public List<PlayedGame> GetAllGames(Game game)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.Find(Query<PlayedGame>.Where(s => s.Game == game)).ToList();
        }

        public List<PlayedGame> GetAllGamesByUsername(string username)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            return collection.Find(Query<PlayedGame>.Where(g => g.Players.Contains(username))).ToList();
        }

        public void UpdateGameRatingById(PlayedGame game)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Save(game);
        }

        public List<PlayedGame> GetLastXPlayedGames(int numberOfGames, string boundAccount)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            List<PlayedGame> result =
                collection.Find(Query<PlayedGame>.Where(game => game.BoundAccount == boundAccount)).OrderByDescending(
                    s => s.TimeStamp).Take(numberOfGames).ToList();

            foreach (PlayedGame game in result)
            {
                game.TimeStamp = GetCurrentTime(game.TimeStamp);
            }

            return result;
        }

        public void UpdateMatch(PlayedGame game)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");
            collection.Save(game);
        }

        public List<PlayerMatchStatistics> GetPlayerStatistics(string username)
        {
            MongoCollection<PlayedGame> collection = _mongoDatabase.GetCollection<PlayedGame>("PlayedGames");

            MongoCursor<PlayedGame> games = collection.Find(Query<PlayedGame>.Where(s => s.Players.Contains(username)));

            var result = new List<PlayerMatchStatistics>();

            return
                (from game in games let numberOfPlayers = game.Players.Count where numberOfPlayers == 2 select game)
                    .Aggregate(result,
                               (current, game) =>
                               UpdatePlayerStatistics(current,
                                                      game.Players[0] == username ? game.Players[1] : game.Players[0],
                                                      game.WinnerUsersnames.Contains(username), game.Game));
        }

        public DateTime GetCurrentTime(DateTime datetime)
        {
            TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            DateTime zoneSpecificTime = TimeZoneInfo.ConvertTime(datetime, zoneInfo);
            return zoneSpecificTime;
        }

        private List<PlayerMatchStatistics> UpdatePlayerStatistics(List<PlayerMatchStatistics> statistics,
                                                                   string username, bool won, Game game)
        {
            var result = statistics.ToList();
            var found = false;

            foreach (PlayerMatchStatistics statistic in statistics)
            {
                if (statistic.Username == username && statistic.Game == game)
                {
                    found = true;

                    if (won)
                    {
                        statistic.Score[0]++;
                    }
                    else
                    {
                        statistic.Score[1]++;
                    }
                    break;
                }
            }

            if (!found)
            {
                var temp = new PlayerMatchStatistics {Game = game, Score = new int[2], Username = username};
                
                if (won)
                {
                    temp.Score[0]++;
                }
                else
                {
                    temp.Score[1]++;
                }
                result.Add(temp);
            }

            return result;
        }
    }
}