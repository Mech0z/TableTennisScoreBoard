using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IMatchManagementRepository
    {
        void CreateMatch(PlayedGame game);
        List<PlayedGame> GetAllGames();
        List<PlayedGame> GetAllGamesByUsername(string username);
        void UpdateGameRatingById(PlayedGame game);
        List<PlayedGame> GetLastXPlayedGames(int numberOfGames, string boundAccount);
        void UpdateMatch(PlayedGame game);
    }
}