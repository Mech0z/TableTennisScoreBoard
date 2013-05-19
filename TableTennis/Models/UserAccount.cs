using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TableTennis.Models
{
    public class UserAccount
    {
        [BsonId]
        public string Username { get; set; }

        public string Password { get; set; }
        public List<UserAccountRole> Roles { get; set; }

        public UserAccount()
        {
            Roles = new List<UserAccountRole>();
        }
    }
}