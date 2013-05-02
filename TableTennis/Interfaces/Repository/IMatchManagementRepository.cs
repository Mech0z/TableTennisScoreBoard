using System;
using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IMatchManagementRepository
    {
        void CreateMatch(PlayedGame game);
        int GetPlayerRatingByPlayerId(Guid playerId);
        List<PlayedGame> GetAllGames();
        List<PlayedGame> GetAllGamesByPlayerID(Guid playerID);
    }
}