using System;
using TableTennis.Models;

namespace TableTennis.Authentication.MongoDB
{
    public interface IMongoMatchManagement
    {
        void CreateMatch(PlayedGame game);
        int GetPlayerRatingByPlayerId(Guid playerId);
    }
}