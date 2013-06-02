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
            var allGames = _matchManagementRepository.GetAllGames();
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

                if (game.Players[0] == game.WinnerUsername)
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
            var allGames = _matchManagementRepository.GetAllGames();
            var players = _playerManagementRepository.GetAllPlayers();

            foreach (var player in players)
            {
                if (player.Ratings.ContainsKey(Game.DoubleTableTennis))
                {
                    player.Ratings[Game.SingleTableTennis] = 1500;
                }
            }
        }
    }
}