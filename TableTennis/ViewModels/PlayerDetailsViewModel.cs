using TableTennis.Models;

namespace TableTennis.ViewModels
{
    public class PlayerDetailsViewModel
    {
        public Player Player { get; set; }
        public PlayedGamesViewModel PlayedGamesViewModel { get; set; }
    }
}