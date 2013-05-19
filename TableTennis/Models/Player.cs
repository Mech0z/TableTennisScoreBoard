using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id, Rating")]
    public class Player
    {
        public Player()
        {
            Rating = 1500;
        }

        [BsonId]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public int Rating { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string GravatarEmail { get; set; }
    }
}