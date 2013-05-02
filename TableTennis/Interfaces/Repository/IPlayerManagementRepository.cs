using System;
using System.Collections.Generic;
using TableTennis.Models;
using TableTennis.Models.Views.PlayerManagement;

namespace TableTennis.Interfaces.Repository
{
    public interface IPlayerManagementRepository
    {
        bool CreatePlayer(Player player);
        List<Player> GetAllPlayers();
        Player GetPlayerById(Guid playerId);
        int GetPlayerRatingById(Guid playerId);
        void UpdateRating(Guid playerId, int rating);
        List<PlayerUsername> GetPlayerUsernames(List<Guid> playerIds);
    }
}