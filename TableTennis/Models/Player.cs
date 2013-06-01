using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using TableTennis.HelperClasses;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id, Rating")]
    public class Player
    {
        public Player()
        {
            Ratings = new Dictionary<Game, int>();
        }

        [BsonId]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public Dictionary<Game, int> Ratings { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string GravatarEmail { get; set; }
    }
}