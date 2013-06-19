using System.Collections.Generic;
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

        public void RecalculateSingleRatings(Game game)
        {
            IOrderedEnumerable<PlayedGame> allGames =
                _matchManagementRepository.GetAllGames(game).OrderBy(games => games.TimeStamp);
            List<Player> players = _playerManagementRepository.GetAllPlayers();

            foreach (Player player in players)
            {
                if (player.Ratings.ContainsKey(game))
                {
                    player.Ratings[game] = 1500;
                }
            }

            foreach (PlayedGame playedGame in allGames)
            {
                if (!playedGame.Ranked)
                {
                    continue;
                }
                Player player1 = players.SingleOrDefault(s => s.Username == playedGame.Players[0]);
                Player player2 = players.SingleOrDefault(s => s.Username == playedGame.Players[1]);
                double rating;

                if (playedGame.WinnerUsersnames.Contains(playedGame.Players[0]))
                {
                    var elo = new EloRating();
                    rating = elo.CalculateRating(player1.Ratings[game],
                                                 player2.Ratings[game], true);

                    player1.Ratings[game] += (int) rating;
                    player2.Ratings[game] -= (int) rating;
                }
                else
                {
                    var elo = new EloRating();
                    rating = elo.CalculateRating(player1.Ratings[game],
                                                 player2.Ratings[game], false);

                    player1.Ratings[game] -= (int) rating;
                    player2.Ratings[game] += (int) rating;
                }

                playedGame.EloPoints = (int) rating;

                _matchManagementRepository.UpdateGameRatingById(playedGame);
            }

            foreach (Player player in players)
            {
                if (player.Ratings.ContainsKey(game))
                {
                    _playerManagementRepository.UpdateRating(player.Username, player.Ratings[game],
                                                             game);
                }
            }
        }

        public void RecalculateDoubleRatings(Game game)
        {
            IOrderedEnumerable<PlayedGame> allGames =
                _matchManagementRepository.GetAllGames(game).OrderBy(games => games.TimeStamp);
            List<Player> players = _playerManagementRepository.GetAllPlayers();

            foreach (Player player in players)
            {
                if (player.Ratings.ContainsKey(game))
                {
                    player.Ratings[game] = 1500;
                }
            }
            foreach (PlayedGame playedGame in allGames)
            {
                if (!playedGame.Ranked)
                {
                    continue;
                }
                Player player1 = players.SingleOrDefault(s => s.Username == playedGame.Players[0]);
                Player player2 = players.SingleOrDefault(s => s.Username == playedGame.Players[1]);
                Player player3 = players.SingleOrDefault(s => s.Username == playedGame.Players[2]);
                Player player4 = players.SingleOrDefault(s => s.Username == playedGame.Players[3]);
                double rating;

                int team1rating = (player1.Ratings[game] + player2.Ratings[game])/2;
                int team2rating = (player3.Ratings[game] + player4.Ratings[game])/2;

                if (playedGame.WinnerUsersnames.Contains(playedGame.Players[0]))
                {
                    var elo = new EloRating();
                    rating = elo.CalculateRating(team1rating, team2rating, true);

                    player1.Ratings[game] += (int) rating;
                    player2.Ratings[game] += (int) rating;
                    player3.Ratings[game] -= (int) rating;
                    player4.Ratings[game] -= (int) rating;
                }
                else
                {
                    var elo = new EloRating();
                    rating = elo.CalculateRating(team1rating, team2rating, false);

                    player1.Ratings[game] -= (int) rating;
                    player2.Ratings[game] -= (int) rating;
                    player3.Ratings[game] += (int) rating;
                    player4.Ratings[game] += (int) rating;
                }

                playedGame.EloPoints = (int) rating;

                _matchManagementRepository.UpdateGameRatingById(playedGame);
            }
            foreach (Player player in players)
            {
                if (player.Ratings.ContainsKey(game))
                {
                    _playerManagementRepository.UpdateRating(player.Username, player.Ratings[game],
                                                             game);
                }
            }
        }
    }
}