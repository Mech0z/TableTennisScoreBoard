using System;
using System.Collections.Generic;
using System.Linq;
using TableTennis.Models.Views.PlayerManagement;

namespace TableTennis.Models
{
    public class PlayedGamesViewModel
    {
        public List<GameViewModel> GameList { get; set; }
        public List<PlayerUsername> PlayerUsernames { get; set; } 

        public PlayedGamesViewModel(List<PlayedGame> playedGames, List<PlayerUsername> opponentList, Guid playerID)
        {
            GameList = new List<GameViewModel>();

            foreach (var game in playedGames)
            {
                var opponentPlayerID = game.PlayerIds[0] == playerID ? game.PlayerIds[1] : game.PlayerIds[0]; 

                var gameVM = new GameViewModel()
                    {
                        TimeStamp = game.TimeStamp,
                        Opponent = opponentList.First(s => s.PlayerID == opponentPlayerID).PlayerUserName,
                        Rating = game.EloPoints,
                        Won = game.WinnerId == playerID ? true : false
                    };

                GameList.Add(gameVM);
            }

            GameList.OrderByDescending(s => s.TimeStamp);
        }
    }
}