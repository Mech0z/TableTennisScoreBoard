using System;
using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IMatchManagementRepository
    {
        void CreateMatch(PlayedGame game);
        List<PlayedGame> GetAllGames();
        List<PlayedGame> GetAllGamesByPlayerID(Guid playerID);
        void UpdateGameRatingById(PlayedGame game);
        List<PlayedGame> GetLastXPlayedGames(int numberOfGames);
    }
}