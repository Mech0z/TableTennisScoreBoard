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
                case Game.SingleTableTennis:
                case Game.DoubleTableTennis:
                    return ValidateTableTennis(gameType, gameSets, out errorMessage);
                case Game.SingleFoosball:
                case Game.DoubleFoosball:
                    return ValidateFoosball(gameType, gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game";
                    return -1;
            }
        }

        private static int ValidateFoosball(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (gameType)
            {
                case GameType.Single11:
                case GameType.Double:
                    return ValidateStandardFoosballGame(gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game rules";
                    return -1;
            }
        }

        private static int ValidateTableTennis(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            int player1Sets = 0;
            int player2Sets = 0;
            int valid = 0;

            int scoreGoal = 0;

            errorMessage = "";

            switch (gameType)
            {
                case GameType.Freestyle:
                    if (gameSets[0].Score1 == 1)
                    {
                        return 1;
                    }
                    if (gameSets[0].Score2 == 1)
                    {
                        return 2;
                    }

                    errorMessage = "One player must have a score of 1 in first set to indicate a winner";
                    return -1;
                case GameType.Single11:
                    scoreGoal = 11;
                    break;
                case GameType.Single21:
                case GameType.Double:
                    scoreGoal = 21;
                    break;
                default:
                    errorMessage = "Invalid game type";
                    break;
            }

            foreach (GameSet gameSet in gameSets)
            {
                if (gameSet.Score1 == scoreGoal && gameSet.Score2 + 2 <= gameSet.Score1)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == scoreGoal && gameSet.Score1 + 2 <= gameSet.Score2)
                {
                    player2Sets++;
                }
                else if (gameSet.Score1 == gameSet.Score2 + 2 && gameSet.Score1 > scoreGoal)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == gameSet.Score1 + 2 && gameSet.Score2 > scoreGoal)
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
                    errorMessage = string.Format("Unvalid set, games are played to {0} or until won by 2 points",
                                                 scoreGoal);
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

        private static int ValidateStandardFoosballGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";

            if (gameSets[0].Score1 == 10 && gameSets[0].Score2 >= 0 && gameSets[0].Score2 <= 9)
            {
                return 1;
            }
            if (gameSets[0].Score2 == 10 && gameSets[0].Score1 >= 0 && gameSets[0].Score1 <= 9)
            {
                return 2;
            }

            errorMessage = "Games are played to 10";
            return -1;
        }
    }

    public enum Game
    {
        SingleTableTennis,
        DoubleTableTennis,
        SingleFoosball,
        DoubleFoosball
    }

    public enum GameType
    {
        Single11,
        Single21,
        Double,
        Freestyle,
        Single
    }
}