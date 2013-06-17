using System.Collections.Generic;
using TableTennis.HelperClasses;
using TableTennis.Models;

namespace TableTennis.Interfaces.Repository
{
    public interface IMatchManagementRepository
    {
        void CreateMatch(PlayedGame game);
        List<PlayedGame> GetAllGames();
        List<PlayedGame> GetAllGames(Game game);
        List<PlayedGame> GetAllGamesByUsername(string username);
        void UpdateGameRatingById(PlayedGame game);
        List<PlayedGame> GetLastXPlayedGames(int numberOfGames, string boundAccount);
        void UpdateMatch(PlayedGame game);
        List<PlayerMatchStatistics> GetPlayerStatistics(string username);
    }
}