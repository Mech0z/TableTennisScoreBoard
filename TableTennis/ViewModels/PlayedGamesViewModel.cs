using System.Collections.Generic;
using System.Linq;
using TableTennis.Models;

namespace TableTennis.ViewModels
{
    public class PlayedGamesViewModel
    {
        public PlayedGamesViewModel(List<PlayedGame> playedGames, string Username)
        {
            GameList = playedGames.OrderByDescending(s => s.TimeStamp).Take(10).ToList();
            PlayerName = Username;
        }

        public List<PlayedGame> GameList { get; set; }
        public string PlayerName { get; set; }
    }
}