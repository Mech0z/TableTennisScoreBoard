using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IPlayerManagementRepository
    {
        bool CreatePlayer(Player player);
        List<Player> GetAllPlayers();
        Player GetPlayerByUsername(string username);
        int GetPlayerRatingByUsername(string username);
        void UpdateRating(string username, int rating);
        void UpdatePlayer(Player player);
    }
}