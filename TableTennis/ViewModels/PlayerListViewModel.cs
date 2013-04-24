using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.ViewModels
{
    public class PlayerListViewModel
    {
        public PlayerListViewModel()
        {
            PlayerList = new List<Player>();
        }

        public List<Player> PlayerList { get; set; } 
    }
}