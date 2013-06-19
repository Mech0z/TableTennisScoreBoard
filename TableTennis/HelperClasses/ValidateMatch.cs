using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.HelperClasses
{
    public static class ValidateMatch
    {
        public static int ValidateGame(Game game, GameType gameType, List<GameSet> gameSets,
                                    out string errorMessage)
        {
            int player1Sets = 0;
            int player2Sets = 0;
            int valid = 0;

            int winSets = GetSets(game, gameType);
            int scoreGoal = GetScoreGoal(game, gameType);
            int winMargin = GetWinnerMargin(game, gameType);

            errorMessage = "";


            foreach (GameSet gameSet in gameSets)
            {
                if (gameSet.Score1 < 0 || gameSet.Score2 < 0)
                {
                    valid = -1;
                    errorMessage = "Scores cant be less than zero!";
                }
                if (gameSet.Score1 == scoreGoal && gameSet.Score2 + winMargin <= gameSet.Score1)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == scoreGoal && gameSet.Score1 + winMargin <= gameSet.Score2)
                {
                    player2Sets++;
                }
                else if (gameSet.Score1 == gameSet.Score2 + winMargin && gameSet.Score1 > scoreGoal)
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 == gameSet.Score1 + winMargin && gameSet.Score2 > scoreGoal)
                {
                    player2Sets++;
                }
                else
                {
                    valid = -1;
                    errorMessage = string.Format("Unvalid set, games are played to {0} or until won by {1} points",
                                                 scoreGoal, winMargin);
                }
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                if (player1Sets == winSets)
                {
                    return 1;
                }
                if (player2Sets == winSets)
                {
                    return 2;
                }

                errorMessage = string.Format("No player have won {0} sets", winSets);
                valid = -1;
            }
            return valid;
        }

        private static int GetWinnerMargin(Game game, GameType gameType)
        {
            switch (game)
            {
                case Game.SingleFoosball:
                    switch (gameType)
                    {
                        case GameType.Single:
                            return 1;
                        case GameType.Double3_10:
                            return 1;
                    }
                    break;
                case Game.DoubleFoosball:
                    switch (gameType)
                    {
                        case GameType.Double:
                            return 1;
                        case GameType.Double3_10:
                            return 1;
                    }
                    break;
                case Game.SingleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single11:
                        case GameType.Single21:
                            return 2;
                    }
                    break;
                case Game.DoubleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single21:
                            return 2;
                    }
                    break;
            }

            return -1;
        }

        private static int GetScoreGoal(Game game, GameType gameType)
        {
            switch (game)
            {
                case Game.SingleFoosball:
                case Game.DoubleFoosball:
                    return 10;
                case Game.SingleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single11:
                            return 11;
                        case GameType.Single21:
                            return 21;
                    }
                    break;
                case Game.DoubleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single21:
                            return 21;
                    }
                    break;
            }

            return -1;
        }

        private static int GetSets(Game game, GameType gameType)
        {
            switch (game)
            {
                case Game.SingleFoosball:
                    switch (gameType)
                    {
                        case GameType.Single:
                            return 1;
                        case GameType.Double3_10:
                            return 2;
                    }
                    break;
                case Game.DoubleFoosball:
                    switch (gameType)
                    {
                        case GameType.Double:
                            return 1;
                        case GameType.Double3_10:
                            return 2;
                    }
                    break;
                case Game.SingleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single11:
                            return 2;
                        case GameType.Single21:
                            return 2;
                    }
                    break;
                case Game.DoubleTableTennis:
                    switch (gameType)
                    {
                        case GameType.Freestyle:
                            return 1;
                        case GameType.Single21:
                            return 2;
                    }
                    break;
            }

            return -1;
        }
    }
}