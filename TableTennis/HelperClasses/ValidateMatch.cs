using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.HelperClasses
{
    public static class ValidateMatch
    {
        public static int ValidateGame(Game game, GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (game)
            {
                case Game.TableTennis:
                    return ValidateTableTennis(gameType, gameSets, out errorMessage);
                case Game.Foosball:
                    return ValidateFoosball(gameType, gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game";
                    return -1;
            }
        }

        private static int ValidateTableTennis(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (gameType)
            {
                case GameType.Standard:
                    return ValidateStandardTTGame(gameSets, out errorMessage);
                case GameType.Double:
                    return ValidateDoubleTTGame(gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid gametype";
                    return -1;
            }
        }

        private static int ValidateFoosball(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (gameType)
            {
                case GameType.Standard:
                    return ValidateStandardFoosballGame(gameSets, out errorMessage);
                case GameType.Double:
                    return ValidateDoubleFoosballGame(gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game rules";
                    return -1;
            }
        }

        private static int ValidateStandardTTGame(List<GameSet> gameSets, out string errorMessage)
        {
            var player1Sets = 0;
            var player2Sets = 0;
            var valid = 0;
            errorMessage = "";

            foreach (var gameSet in gameSets)
            {
                if (gameSet.Score1 == 11 && gameSet.Score2 <= 9)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == 11 && gameSet.Score1 <= 9)
                {
                    player2Sets++;
                }
                else if(gameSet.Score1 == gameSet.Score2 + 2  && gameSet.Score1 > 11)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == gameSet.Score1 + 2 && gameSet.Score2 > 11)
                {
                    player2Sets++;
                }
                else if (gameSet.Score1 < 0 || gameSet.Score2 < 0)
                {
                    valid = -1;
                    errorMessage = "Scores cant be less than zero!";
                }
                else
                {
                    valid = -1;
                    errorMessage = "Unvalid set, games are played to 11 or until won by 2 points";
                }
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                if (player1Sets == 2)
                {
                    return 1;
                }
                if (player2Sets == 2)
                {
                    return 2;
                }

                errorMessage = "No player have won 2 sets";
                valid = -1;
            }
            return valid;
        }

        private static int ValidateDoubleTTGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";
            return 0;
        }

        private static int ValidateStandardFoosballGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";
            return 0;
        }

        private static int ValidateDoubleFoosballGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";
            return 0;
        }
    }

    public enum Game
    {
        TableTennis,
        Foosball
    }

    public enum GameType
    {
        Standard,
        Double
    }
}