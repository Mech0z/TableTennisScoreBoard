using System.Diagnostics;
using System.Linq;
using TableTennis.Interfaces.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;

namespace TableTennis.HelperClasses
{
    public class RatingCalculator : IRatingCalculator
    {
        private readonly IMatchManagementRepository _matchManagementRepository;
        private readonly IPlayerManagementRepository _playerManagementRepository;

        public RatingCalculator(IMatchManagementRepository matchManagementRepository,
                                IPlayerManagementRepository playerManagementRepository)
        {
            _matchManagementRepository = matchManagementRepository;
            _playerManagementRepository = playerManagementRepository;
        }

        public void RecalculateSingleTTRatings()
        {
            var allGames = _matchManagementRepository.GetAllGames(Game.SingleTableTennis).OrderBy(games => games.TimeStamp);
            var players = _playerManagementRepository.GetAllPlayers();

            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.SingleTableTennis))
                {
                    player.Ratings[Game.SingleTableTennis] = 1500;
                }
            }

            foreach (var game in allGames)
            {
                if (!game.Ranked)
                {
                    continue;
                }
                var player1 = players.SingleOrDefault(s => s.Username == game.Players[0]);
                var player2 = players.SingleOrDefault(s => s.Username == game.Players[1]);
                int rating;

                if (game.WinnerUsersnames.Contains(game.Players[0]))
                {
                    var elo = new EloRating(player1.Ratings[Game.SingleTableTennis], player2.Ratings[Game.SingleTableTennis], 1, 0);
                    player1.Ratings[Game.SingleTableTennis] += (int)elo.Point1;
                    player2.Ratings[Game.SingleTableTennis] += (int)elo.Point2;
                    rating = (int) elo.Point1;
                }
                else
                {
                    var elo = new EloRating(player1.Ratings[Game.SingleTableTennis], player2.Ratings[Game.SingleTableTennis], 0, 1);
                    player1.Ratings[Game.SingleTableTennis] += (int)elo.Point1;
                    player2.Ratings[Game.SingleTableTennis] += (int)elo.Point2;
                    rating = (int) elo.Point2;
                }

                game.EloPoints = rating;

                _matchManagementRepository.UpdateGameRatingById(game);
            }

            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.SingleTableTennis))
                {
                    _playerManagementRepository.UpdateRating(player.Username, player.Ratings[Game.SingleTableTennis],
                                                             Game.SingleTableTennis);
                }
            }
        }

        public void RecalculateDoubleTTRatings()
        {
            var allGames = _matchManagementRepository.GetAllGames(Game.DoubleTableTennis).OrderBy(games => games.TimeStamp);
            var players = _playerManagementRepository.GetAllPlayers();

            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.DoubleTableTennis))
                {
                    player.Ratings[Game.DoubleTableTennis] = 1500;
                }
            }
            foreach (var game in allGames)
            {
                if (!game.Ranked)
                {
                    continue;
                }
                var player1 = players.SingleOrDefault(s => s.Username == game.Players[0]);
                var player2 = players.SingleOrDefault(s => s.Username == game.Players[1]);
                var player3 = players.SingleOrDefault(s => s.Username == game.Players[2]);
                var player4 = players.SingleOrDefault(s => s.Username == game.Players[3]);
                int rating;

                if (game.WinnerUsersnames.Contains(game.Players[0]))
                {
                    var team1rating = player1.Ratings[Game.DoubleTableTennis] + player2.Ratings[Game.DoubleTableTennis] / 2;
                    var team2rating = player3.Ratings[Game.DoubleTableTennis] + player4.Ratings[Game.DoubleTableTennis] / 2;
                    var elo = new EloRating(team1rating, team2rating, 1, 0);
                    player1.Ratings[Game.DoubleTableTennis] += (int)elo.Point1;
                    player2.Ratings[Game.DoubleTableTennis] += (int)elo.Point1;
                    player3.Ratings[Game.DoubleTableTennis] += (int)elo.Point2;
                    player4.Ratings[Game.DoubleTableTennis] += (int)elo.Point2;
                    rating = (int)elo.Point1;
                }
                else
                {
                    var team1rating = player1.Ratings[Game.DoubleTableTennis] + player2.Ratings[Game.DoubleTableTennis] / 2;
                    var team2rating = player3.Ratings[Game.DoubleTableTennis] + player4.Ratings[Game.DoubleTableTennis] / 2;
                    var elo = new EloRating(team1rating, team2rating, 0, 1);
                    player1.Ratings[Game.DoubleTableTennis] += (int)elo.Point1;
                    player2.Ratings[Game.DoubleTableTennis] += (int)elo.Point1;
                    player3.Ratings[Game.DoubleTableTennis] += (int)elo.Point2;
                    player4.Ratings[Game.DoubleTableTennis] += (int)elo.Point2;
                    rating = (int)elo.Point2;
                }

                game.EloPoints = rating;

                _matchManagementRepository.UpdateGameRatingById(game);
            }
            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.DoubleTableTennis))
                {
                    _playerManagementRepository.UpdateRating(player.Username, player.Ratings[Game.DoubleTableTennis],
                                                             Game.DoubleTableTennis);
                }
            }
        }


        public void RecalculateDoubleFoosballRatings()
        {
            var allGames = _matchManagementRepository.GetAllGames(Game.DoubleFoosball).OrderBy(games => games.TimeStamp);
            var players = _playerManagementRepository.GetAllPlayers();

            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.DoubleFoosball))
                {
                    player.Ratings[Game.DoubleFoosball] = 1500;
                }
            }
            foreach (var game in allGames)
            {
                if (!game.Ranked)
                {
                    continue;
                }
                var player1 = players.SingleOrDefault(s => s.Username == game.Players[0]);
                var player2 = players.SingleOrDefault(s => s.Username == game.Players[1]);
                var player3 = players.SingleOrDefault(s => s.Username == game.Players[2]);
                var player4 = players.SingleOrDefault(s => s.Username == game.Players[3]);
                int rating;


                if (game.WinnerUsersnames.Contains(game.Players[0]))
                {
                    var team1rating = player1.Ratings[Game.DoubleFoosball] + player2.Ratings[Game.DoubleFoosball] / 2;
                    var team2rating = player3.Ratings[Game.DoubleFoosball] + player4.Ratings[Game.DoubleFoosball] / 2;
                    var elo = new EloRating(team1rating, team2rating, 1, 0);
                    player1.Ratings[Game.DoubleFoosball] += (int)elo.Point1;
                    player2.Ratings[Game.DoubleFoosball] += (int)elo.Point1;
                    player3.Ratings[Game.DoubleFoosball] += (int)elo.Point2;
                    player4.Ratings[Game.DoubleFoosball] += (int)elo.Point2;
                    rating = (int)elo.Point1;
                }
                else
                {
                    var team1rating = player1.Ratings[Game.DoubleFoosball] + player2.Ratings[Game.DoubleFoosball] / 2;
                    var team2rating = player3.Ratings[Game.DoubleFoosball] + player4.Ratings[Game.DoubleFoosball] / 2;
                    var elo = new EloRating(team1rating, team2rating, 0, 1);
                    player1.Ratings[Game.DoubleFoosball] += (int)elo.Point1;
                    player2.Ratings[Game.DoubleFoosball] += (int)elo.Point1;
                    player3.Ratings[Game.DoubleFoosball] += (int)elo.Point2;
                    player4.Ratings[Game.DoubleFoosball] += (int)elo.Point2;
                    rating = (int)elo.Point2;
                }

                game.EloPoints = rating;
                _matchManagementRepository.UpdateGameRatingById(game);
            }
            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.DoubleFoosball))
                {
                    _playerManagementRepository.UpdateRating(player.Username, player.Ratings[Game.DoubleFoosball],
                                                             Game.DoubleFoosball);
                }
            }
        }
    }
}