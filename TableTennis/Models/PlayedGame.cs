using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id")]
    public class PlayedGame
    {
        public PlayedGame()
        {
            Players = new List<Player>();
        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        public bool Ranked { get; set; }

        public List<Player> Players { get; set; }

        public int WinnerId { get; set; }

        public int EloPoints { get; set; }
    }
}