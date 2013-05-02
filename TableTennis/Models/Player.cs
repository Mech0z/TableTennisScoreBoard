using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id, Rating")]
    public class Player
    {
        public Player()
        {
            Rating = 1500;
        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Required]
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