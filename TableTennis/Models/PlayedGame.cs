using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TableTennis.HelperClasses;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id, GameSets")]
    public class PlayedGame
    {
        public PlayedGame()
        {
            Players = new List<string>();
            GameSets = new List<GameSet>();
            WinnerUsersnames = new List<string>();
        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        public bool Ranked { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<string> Players { get; set; }

        public List<string> WinnerUsersnames { get; set; }

        public int EloPoints { get; set; }

        public List<GameSet> GameSets { get; set; }

        public string BoundAccount { get; set; }

        public GameType GameType { get; set; }

        public Game Game { get; set; }
    }
}