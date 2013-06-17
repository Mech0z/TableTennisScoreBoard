using TableTennis.HelperClasses;

namespace TableTennis.Models
{
    public class PlayerMatchStatistics
    {
        public string Username { get; set; }
        public Game Game { get; set; }
        public int[] Score { get; set; }
    }
}