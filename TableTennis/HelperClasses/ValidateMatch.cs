﻿using System.Collections.Generic;
using TableTennis.Models;

namespace TableTennis.HelperClasses
{
    public static class ValidateMatch
    {
        public static bool ValidateGame(Game game, GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (game)
            {
                case Game.TableTennis:
                    return ValidateTableTennis(gameType, gameSets, out errorMessage);
                case Game.Foosball:
                    return ValidateFoosball(gameType, gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game";
                    return false;
            }
        }

        private static bool ValidateTableTennis(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (gameType)
            {
                case GameType.Standard:
                    return ValidateStandardTTGame(gameSets, out errorMessage);
                case GameType.Double:
                    return ValidateDoubleTTGame(gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid gametype";
                    return false;
            }
        }

        private static bool ValidateFoosball(GameType gameType, List<GameSet> gameSets, out string errorMessage)
        {
            switch (gameType)
            {
                case GameType.Standard:
                    return ValidateStandardFoosballGame(gameSets, out errorMessage);
                case GameType.Double:
                    return ValidateDoubleFoosballGame(gameSets, out errorMessage);
                default:
                    errorMessage = "Not valid game rules";
                    return false;
            }
        }

        private static bool ValidateStandardTTGame(List<GameSet> gameSets, out string errorMessage)
        {
            var player1Sets = 0;
            var player2Sets = 0;
            var valid = true;
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
                else if(gameSet.Score1 + 2 == gameSet.Score2 )
                {
                    player1Sets++;
                }
                else if (gameSet.Score2 + 2 == gameSet.Score1)
                {
                    player2Sets++;
                }
                else
                {
                    valid = false;
                    errorMessage = "Unvalid set, games are played to 11 or until won by 2 points";
                }

            }

            return valid;
        }

        private static bool ValidateDoubleTTGame(List<GameSet> gameSets, out string errorMessage)
        {
            var setCount = gameSets.Count;

            errorMessage = "";
            return true;
        }

        private static bool ValidateStandardFoosballGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        private static bool ValidateDoubleFoosballGame(List<GameSet> gameSets, out string errorMessage)
        {
            errorMessage = "";
            return true;
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