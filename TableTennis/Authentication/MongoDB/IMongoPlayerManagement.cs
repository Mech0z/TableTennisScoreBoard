using System;
using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.Authentication.MongoDB
{
    public interface IMongoPlayerManagement
    {
        bool CreatePlayer(Player player);
        List<Player> GetAllPlayers();
        Player GetPlayerById(Guid playerId);
        int GetPlayerRatingById(Guid playerId);
    }
}