using System;
using System.Collections.Generic;
using TableTennis.HelperClasses;

namespace TableTennis.Models.Views.PlayerManagement
{
    public class GameViewModel
    {
        public string Opponent { get; set; }
        public bool Won { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Rating { get; set; }
        public List<GameSet> GameSets { get; set; }
    }
}