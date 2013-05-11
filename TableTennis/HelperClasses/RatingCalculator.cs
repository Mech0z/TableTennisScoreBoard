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

        public void RecalculateRatings()
        {
            var allGames = _matchManagementRepository.GetAllGames();
            var players = _playerManagementRepository.GetAllPlayers();

            foreach (var player in players)
            {
                player.Rating = 1500;
            }

            foreach (var game in allGames)
            {
                if (!game.Ranked)
                {
                    continue;
                }
                var player1 = players.SingleOrDefault(s => s.Id == game.PlayerIds[0]);
                var player2 = players.SingleOrDefault(s => s.Id == game.PlayerIds[1]);
                int rating;

                if (game.PlayerIds[0] == game.WinnerId)
                {
                    var elo = new EloRating(player1.Rating, player2.Rating, 1, 0);
                    player1.Rating += (int) elo.Point1;
                    player2.Rating += (int) elo.Point2;
                    rating = (int) elo.Point1;
                }
                else
                {
                    var elo = new EloRating(player1.Rating, player2.Rating, 0, 1);
                    player1.Rating += (int) elo.Point1;
                    player2.Rating += (int) elo.Point2;
                    rating = (int) elo.Point2;
                }

                game.EloPoints = rating;

                _matchManagementRepository.UpdateGameRatingById(game);
            }

            foreach (var player in players)
            {
                _playerManagementRepository.UpdateRating(player.Id, player.Rating);
            }
        }
    }
}