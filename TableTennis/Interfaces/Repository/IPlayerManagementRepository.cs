using System.Collections.Generic;
using TableTennis.HelperClasses;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IPlayerManagementRepository
    {
        bool CreatePlayer(Player player);
        List<Player> GetAllPlayers();
        Player GetPlayerByUsername(string username);
        int GetPlayerRatingByUsername(string username, Game game);
        void UpdateRating(string username, int rating, Game game);
        void UpdatePlayer(Player player);
    }
}