using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.ViewModels
{
    public class PlayerDetailsViewModel
    {
        public Player Player { get; set; }
        public PlayedGamesViewModel PlayedGamesViewModel { get; set; }
        public List<PlayerMatchStatistics> PlayedMatchStatistics { get; set; }
    }
}