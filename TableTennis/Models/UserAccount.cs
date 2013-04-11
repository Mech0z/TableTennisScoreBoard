using System.Collections.Generic;

namespace TableTennis.Models
{
    public class UserAccount
    { 
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<UserAccountRole> Roles { get; set; }

        public UserAccount()
        {
            Roles = new List<UserAccountRole>();
        }
    }
}