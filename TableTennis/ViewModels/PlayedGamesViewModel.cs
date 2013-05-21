using System.Collections.Generic;
using System.Linq;
using TableTennis.Models;
using TableTennis.Models.Views.PlayerManagement;

namespace TableTennis.ViewModels
{
    public class PlayedGamesViewModel
    {
        public PlayedGamesViewModel(List<PlayedGame> playedGames, string Username)
        {
            GameList = new List<GameViewModel>();

            foreach (var game in playedGames)
            {
                var opponentUsername = game.Players[0] == Username ? game.Players[1] : game.Players[0];
                var won = game.WinnerUsername == Username;
                var gameSets = game.GameSets ?? new List<GameSet>();

                var gameVM = new GameViewModel
                    {
                        TimeStamp = game.TimeStamp,
                        Opponent = opponentUsername,
                        Won = won,
                        Rating = game.EloPoints,
                        GameSets = gameSets
                    };

                GameList.Add(gameVM);
            }

            GameList = GameList.OrderByDescending(s => s.TimeStamp).Take(10).ToList();
        }

        public List<GameViewModel> GameList { get; set; }
        public List<PlayerUsername> PlayerUsernames { get; set; }
    }
}