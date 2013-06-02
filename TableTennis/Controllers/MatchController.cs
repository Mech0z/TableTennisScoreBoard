﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TableTennis.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;
using TableTennis.ViewModels;

namespace TableTennis.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchManagementRepository _matchManagementRepository;
        private readonly IPlayerManagementRepository _playerManagementRepository;

        public MatchController(IMatchManagementRepository matchManagementRepository,
                               IPlayerManagementRepository playerManagementRepository)
        {
            _matchManagementRepository = matchManagementRepository;
            _playerManagementRepository = playerManagementRepository;
        }

        /// <summary>
        ///     Show 10 last games from account
        /// </summary>
        /// <returns></returns>
        public ActionResult LastGames()
        {
            //TODO var games = _matchManagementRepository.GetLastXPlayedGames(10, HttpContext.User.Identity.Name);
            var vm = new LastGamesViewModel {PlayedGames = _matchManagementRepository.GetLastXPlayedGames(5, "d60")};

            return View(vm);
        }

        //
        // GET: /Match/Create
        [HttpGet]
        public ActionResult Create()
        {
            CreateViewModel vm = CreateMatchVM();
            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateDouble()
        {
            CreateDoubleViewModel vm = CreateDoubleMatchVM();

            return View(vm);
        }

        private CreateViewModel CreateMatchVM()
        {
            var vm = new CreateViewModel
                {
                    PlayerList = CreatePlayerList(),
                    GameTypes = CreateTableTennisGameTypes()
                };

            return vm;
        }

        private CreateDoubleViewModel CreateDoubleMatchVM()
        {
            var vm = new CreateDoubleViewModel
                {
                    PlayerList = CreatePlayerList(),
                    GameTypes = CreateTableTennisGameTypes()
                };

            return vm;
        }

        //
        // POST: /Match/Create

        [HttpPost]
        public ActionResult Create(CreateViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    vm = RecreateSingleTTMatchViewModel(vm, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username)
                {
                    vm = RecreateSingleTTMatchViewModel(vm, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                    {
                        Players = {vm.Player1Username, vm.Player2Username},
                        Ranked = true,
                        TimeStamp = DateTime.UtcNow,
                    };

                game.GameSets.Add(new GameSet {Score1 = vm.Score1Set1, Score2 = vm.Score2Set1});
                if (vm.Score1Set2 != 0 || vm.Score2Set2 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set2, Score2 = vm.Score2Set2});
                }
                if (vm.Score1Set3 != 0 || vm.Score2Set3 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set3, Score2 = vm.Score2Set3});
                }

                //Validate game score
                string errorMessage = "";
                var gameType = (GameType) Enum.Parse(typeof (GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.SingleTableTennis, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateSingleTTMatchViewModel(vm, errorMessage);
                    return View(vm);
                }

                int player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username,
                                                                                          Game.SingleTableTennis);
                int player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username,
                                                                                          Game.SingleTableTennis);

                int playerOneWin = validationResult == 1 ? 1 : 0;
                int playerTwoWin = validationResult == 2 ? 1 : 0;

                var ratingSystem = new EloRating(player1Rating, player2Rating, playerOneWin, playerTwoWin);

                game.EloPoints = playerOneWin == 1 ? (int) ratingSystem.Point1 : (int) ratingSystem.Point2;

                _playerManagementRepository.UpdateRating(vm.Player1Username, (int) ratingSystem.FinalResult1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, (int) ratingSystem.FinalResult2,
                                                         Game.SingleTableTennis);

                game.WinnerUsername = validationResult == 1 ? vm.Player1Username : vm.Player2Username;
                game.Game = Game.SingleTableTennis;
                game.BoundAccount = "d60";
                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerList", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        ///     CreateTabletennisDouble
        /// </summary>
        /// <param name="vm"></param>
        [HttpPost]
        public ActionResult CreateDouble(CreateDoubleViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    vm = RecreateDoubleTTMatchViewModel(vm, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username || vm.Player1Username == vm.Player3Username ||
                    vm.Player1Username == vm.Player4Username || vm.Player2Username == vm.Player3Username ||
                    vm.Player2Username == vm.Player4Username || vm.Player3Username == vm.Player4Username)
                {
                    vm = RecreateDoubleTTMatchViewModel(vm, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                    {
                        Players = {vm.Player1Username, vm.Player2Username, vm.Player3Username, vm.Player4Username},
                        Ranked = true,
                        TimeStamp = DateTime.UtcNow,
                    };

                game.GameSets.Add(new GameSet {Score1 = vm.Score1Set1, Score2 = vm.Score2Set1});
                if (vm.Score1Set2 != 0 || vm.Score2Set2 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set2, Score2 = vm.Score2Set2});
                }
                if (vm.Score1Set3 != 0 || vm.Score2Set3 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set3, Score2 = vm.Score2Set3});
                }

                //Validate game score
                string errorMessage = "";
                var gameType = (GameType) Enum.Parse(typeof (GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.SingleTableTennis, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateDoubleTTMatchViewModel(vm, errorMessage);
                    return View(vm);
                }

                int player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username,
                                                                                          Game.DoubleTableTennis);
                int player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username,
                                                                                          Game.DoubleTableTennis);
                int player3Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player3Username,
                                                                                          Game.DoubleTableTennis);
                int player4Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player4Username,
                                                                                          Game.DoubleTableTennis);

                int playerOneWin = validationResult == 1 ? 1 : 0;
                int playerTwoWin = validationResult == 2 ? 1 : 0;

                var ratingSystem = new EloRating((player1Rating + player2Rating)/2, (player3Rating + player4Rating)/2,
                                                 playerOneWin, playerTwoWin);

                game.EloPoints = playerOneWin == 1 ? (int) ratingSystem.Point1 : (int) ratingSystem.Point2;

                _playerManagementRepository.UpdateRating(vm.Player1Username, (int) ratingSystem.FinalResult1,
                                                         Game.DoubleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, (int) ratingSystem.FinalResult1,
                                                         Game.DoubleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player3Username, (int) ratingSystem.FinalResult2,
                                                         Game.DoubleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player4Username, (int) ratingSystem.FinalResult2,
                                                         Game.DoubleTableTennis);

                game.WinnerUsername = validationResult == 1 ? vm.Player1Username : vm.Player2Username;
                game.Game = Game.DoubleTableTennis;

                game.BoundAccount = "d60";
                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerList", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        ///     Creates a new view model if there is an error when submitting
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private CreateViewModel RecreateSingleTTMatchViewModel(CreateViewModel vm, string errorMessage)
        {
            ModelState.Clear();
            vm.PlayerList = CreatePlayerList();
            vm.GameTypes = CreateTableTennisGameTypes();
            ModelState.AddModelError("ValidationError", errorMessage);

            return vm;
        }

        private IEnumerable<SelectListItem> CreateTableTennisGameTypes()
        {
            return new[]
                {
                    new SelectListItem {Text = "Freestyle", Value = GameType.Freestyle.ToString()},
                    new SelectListItem {Text = "11 Point best of 3", Value = GameType.Single11.ToString()},
                    new SelectListItem {Text = "21 Point best of 3", Value = GameType.Single21.ToString()}
                };
        }

        private CreateDoubleViewModel RecreateDoubleTTMatchViewModel(CreateDoubleViewModel vm, string errorMessage)
        {
            ModelState.Clear();
            vm.PlayerList = CreatePlayerList();
            vm.GameTypes = CreateTableTennisGameTypes();
            ModelState.AddModelError("ValidationError", errorMessage);

            return vm;
        }

        private IEnumerable<SelectListItem> CreatePlayerList()
        {
            List<Player> playerList = _playerManagementRepository.GetAllPlayers().OrderBy(p => p.Username).ToList();
            return playerList.Select(p => new SelectListItem
                {
                    Text = p.Username,
                    Value = p.Username
                });
        }
    }
}