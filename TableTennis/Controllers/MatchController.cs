using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
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
        private readonly IPersistentConnectionContext _hubConnectionContext;

        public MatchController(IMatchManagementRepository matchManagementRepository,
                               IPlayerManagementRepository playerManagementRepository,
            IPersistentConnectionContext hubContext)
        {
            _matchManagementRepository = matchManagementRepository;
            _playerManagementRepository = playerManagementRepository;
            _hubConnectionContext = hubContext;
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
            CreateViewModel vm = CreateMatchVM(Game.SingleTableTennis);
            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateDouble()
        {
            CreateDoubleViewModel vm = CreateDoubleMatchVM(Game.DoubleTableTennis);

            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateSingleFoosball()
        {
            CreateViewModel vm = CreateMatchVM(Game.SingleFoosball);
            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateDoubleFoosball()
        {
            CreateDoubleViewModel vm = CreateDoubleMatchVM(Game.DoubleFoosball);
            return View(vm);
        }

        private CreateViewModel CreateMatchVM(Game game)
        {
            var vm = new CreateViewModel
                {
                    PlayerList = CreatePlayerList(),
                    GameTypes = CreateTableTennisGameTypes(game)
                };

            return vm;
        }

        private CreateDoubleViewModel CreateDoubleMatchVM(Game game)
        {
            var vm = new CreateDoubleViewModel
                {
                    PlayerList = CreatePlayerList(),
                    GameTypes = CreateTableTennisGameTypes(game)
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
                    vm = RecreateSingleViewModel(vm, Game.SingleTableTennis, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username)
                {
                    vm = RecreateSingleViewModel(vm, Game.SingleTableTennis, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                    {
                        Players = {vm.Player1Username, vm.Player2Username},
                        Ranked = true,
                        TimeStamp = DateTime.UtcNow,
                        GameSets = CreateGameSetsSingle(vm),
                        Game = Game.SingleTableTennis,
                        BoundAccount = "d60"
                    };

               //Validate game score
                string errorMessage = "";
                var gameType = (GameType) Enum.Parse(typeof (GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.SingleTableTennis, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateSingleViewModel(vm,Game.SingleTableTennis , errorMessage);
                    return View(vm);
                }

                int player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username,
                                                                                          Game.SingleTableTennis);
                int player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username,
                                                                                          Game.SingleTableTennis);

                bool playerOneWin = validationResult == 1;
               
                BroadCastWinner(vm.Player1Username, vm.Player2Username, "table tennis", playerOneWin);
                
                var elo = new EloRating();
                var rating = elo.CalculateRating(player1Rating, player2Rating, playerOneWin);
                game.EloPoints = (int)rating;
                _playerManagementRepository.UpdateRating(vm.Player1Username,  playerOneWin ? player1Rating + (int) rating : player1Rating + (int) rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, !playerOneWin ? player2Rating + (int) rating : player2Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);

                game.WinnerUsersnames.Add(validationResult == 1 ? vm.Player1Username : vm.Player2Username);
                
                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerList", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        private void BroadCastWinner(string teamOneName, string teamTwoName, string matchName, bool didTeamOneWin)
        {
            var msg = new BroadCastMessage
            {
                Winner = didTeamOneWin ? teamOneName : teamTwoName,
                Looser = didTeamOneWin ? teamTwoName : teamOneName,
                MatchType = matchName
            };

            string message = string.Format("{0} just won a {1} match against {2}", msg.Winner, msg.MatchType, msg.Looser);
            _hubConnectionContext.Connection.Broadcast(message);
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
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleTableTennis, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username || vm.Player1Username == vm.Player3Username ||
                    vm.Player1Username == vm.Player4Username || vm.Player2Username == vm.Player3Username ||
                    vm.Player2Username == vm.Player4Username || vm.Player3Username == vm.Player4Username)
                {
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleTableTennis, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                    {
                        Players = {vm.Player1Username, vm.Player2Username, vm.Player3Username, vm.Player4Username},
                        Ranked = true,
                        TimeStamp = DateTime.UtcNow,
                        Game = Game.DoubleTableTennis,
                        GameSets = CreateGameSets(vm),
                        BoundAccount = "d60"
                    };

                //Validate game score
                string errorMessage = "";
                var gameType = (GameType) Enum.Parse(typeof (GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.SingleTableTennis, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleTableTennis, errorMessage);
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

                bool playerOneWin = validationResult == 1;

                var elo = new EloRating();
                var rating = elo.CalculateRating((player1Rating + player2Rating) / 2, (player3Rating + player4Rating) / 2, playerOneWin);
                game.EloPoints = (int)rating;
                _playerManagementRepository.UpdateRating(vm.Player1Username, playerOneWin ? (int)rating : (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, playerOneWin ? (int)rating : (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player3Username, !playerOneWin ? (int)rating : (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player4Username, !playerOneWin ? (int)rating : (int)rating * -1,
                                                         Game.SingleTableTennis);

                if (validationResult == 1)
                {
                    game.WinnerUsersnames.Add(game.Players[0]);
                    game.WinnerUsersnames.Add(game.Players[1]);
                }
                else
                {
                    game.WinnerUsersnames.Add(game.Players[2]);
                    game.WinnerUsersnames.Add(game.Players[3]);
                }

                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerListTTDouble", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        private List<GameSet> CreateGameSetsSingle(CreateViewModel vm)
        {
            var result = new List<GameSet>
                {
                    new GameSet
                        {
                            Score1 = vm.Score1Set1, 
                            Score2 = vm.Score2Set1
                        }
                };
            if (vm.Score1Set2 != 0 || vm.Score2Set2 != 0)
            {
                result.Add(new GameSet { Score1 = vm.Score1Set2, Score2 = vm.Score2Set2 });
            }
            if (vm.Score1Set3 != 0 || vm.Score2Set3 != 0)
            {
                result.Add(new GameSet { Score1 = vm.Score1Set3, Score2 = vm.Score2Set3 });
            }
            return result;
        }

        private List<GameSet> CreateGameSets(CreateDoubleViewModel vm)
        {
            var result = new List<GameSet>
                {
                    new GameSet 
                    {
                        Score1 = vm.Score1Set1, 
                        Score2 = vm.Score2Set1
                    }
                };

            if (vm.Score1Set2 != 0 || vm.Score2Set2 != 0)
            {
                result.Add(new GameSet {Score1 = vm.Score1Set2, Score2 = vm.Score2Set2});
            }
            if (vm.Score1Set3 != 0 || vm.Score2Set3 != 0)
            {
                result.Add(new GameSet {Score1 = vm.Score1Set3, Score2 = vm.Score2Set3});
            }

            return result;
        }
            
        [HttpPost]
        public ActionResult CreateSingleFoosball(CreateViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    vm = RecreateSingleViewModel(vm, Game.SingleFoosball, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username)
                {
                    vm = RecreateSingleViewModel(vm, Game.SingleFoosball, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                {
                    Players = { vm.Player1Username, vm.Player2Username },
                    Ranked = true,
                    TimeStamp = DateTime.UtcNow,
                    GameSets = CreateGameSetsSingle(vm),
                    Game = Game.SingleFoosball,
                    BoundAccount = "d60"
                };

                //Validate game score
                string errorMessage = "";
                var gameType = (GameType)Enum.Parse(typeof(GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.SingleFoosball, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateSingleViewModel(vm, Game.SingleFoosball, errorMessage);
                    return View(vm);
                }

                int player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username,
                                                                                          Game.SingleFoosball);
                int player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username,
                                                                                          Game.SingleFoosball);

                bool playerOneWin = validationResult == 1;

                var elo = new EloRating();
                var rating = elo.CalculateRating(player1Rating, player2Rating, playerOneWin); 
                game.EloPoints = (int)rating;

                _playerManagementRepository.UpdateRating(vm.Player1Username, playerOneWin ? player1Rating + (int)rating : player1Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, !playerOneWin ? player2Rating + (int)rating : player2Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);

                game.WinnerUsersnames.Add(validationResult == 1 ? vm.Player1Username : vm.Player2Username);
                
                
                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerListSingleFoosball", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateDoubleFoosball(CreateDoubleViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleFoosball, "Failed to submit, invalid data!");
                    return View(vm);
                }
                if (vm.Player1Username == vm.Player2Username || vm.Player1Username == vm.Player3Username ||
                    vm.Player1Username == vm.Player4Username || vm.Player2Username == vm.Player3Username ||
                    vm.Player2Username == vm.Player4Username || vm.Player3Username == vm.Player4Username)
                {
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleFoosball, "Select different players!");
                    return View(vm);
                }

                var game = new PlayedGame
                {
                    Players = { vm.Player1Username, vm.Player2Username, vm.Player3Username, vm.Player4Username },
                    Ranked = true,
                    TimeStamp = DateTime.UtcNow,
                    Game = Game.DoubleFoosball,
                    BoundAccount = "d60",
                    GameSets = CreateGameSets(vm)
                };

                //Validate game score
                string errorMessage = "";
                var gameType = (GameType)Enum.Parse(typeof(GameType), vm.GameType);
                game.GameType = gameType;
                int validationResult = ValidateMatch.ValidateGame(Game.DoubleFoosball, gameType, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    vm = RecreateDoubleViewMdoel(vm, Game.DoubleFoosball, errorMessage);
                    return View(vm);
                }

                int player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username,
                                                                                          Game.DoubleFoosball);
                int player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username,
                                                                                          Game.DoubleFoosball);
                int player3Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player3Username,
                                                                                          Game.DoubleFoosball);
                int player4Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player4Username,
                                                                                          Game.DoubleFoosball);

                bool playerOneWin = validationResult == 1;

                var elo = new EloRating();
                var rating = elo.CalculateRating((player1Rating + player2Rating) / 2, (player3Rating + player4Rating) / 2, playerOneWin);
                game.EloPoints = (int)rating;

                _playerManagementRepository.UpdateRating(vm.Player1Username, playerOneWin ? player1Rating + (int)rating : player1Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player2Username, playerOneWin ? player2Rating + (int)rating : player2Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player3Username, !playerOneWin ? player3Rating + (int)rating : player3Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);
                _playerManagementRepository.UpdateRating(vm.Player4Username, !playerOneWin ? player4Rating + (int)rating : player4Rating + (int)rating * -1,
                                                         Game.SingleTableTennis);


                if (validationResult == 1)
                {
                    game.WinnerUsersnames.Add(game.Players[0]);
                    game.WinnerUsersnames.Add(game.Players[1]);
                }
                else
                {
                    game.WinnerUsersnames.Add(game.Players[2]);
                    game.WinnerUsersnames.Add(game.Players[3]);
                }
                
                //TODO game.BoundAccount = HttpContext.User.Identity.Name;
                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerListDoubleFoosball", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        

        private IEnumerable<SelectListItem> CreateTableTennisGameTypes(Game game)
        {
            switch (game)
            {
                case Game.SingleFoosball:
                    return new[]
                        {
                            new SelectListItem {Text = "Single to 10", Value = GameType.Single.ToString()},
                            new SelectListItem {Text = "10 Point best best of 3", Value = GameType.Double3_10.ToString()}
                        };
                case Game.DoubleFoosball:
                    return new[]
                        {
                            new SelectListItem {Text = "Double to 10", Value = GameType.Double.ToString()},
                            new SelectListItem {Text = "10 Point best best of 3", Value = GameType.Double3_10.ToString()}
                        };
                case Game.DoubleTableTennis:
                    return new[]
                        {
                            new SelectListItem {Text = "Freestyle", Value = GameType.Freestyle.ToString()},
                            new SelectListItem {Text = "21 Point best of 3", Value = GameType.Single21.ToString()}
                        };
                default:
                    return new[]
                        {
                            new SelectListItem {Text = "Freestyle", Value = GameType.Freestyle.ToString()},
                            new SelectListItem {Text = "11 Point best of 3", Value = GameType.Single11.ToString()},
                            new SelectListItem {Text = "21 Point best of 3", Value = GameType.Single21.ToString()}
                        };
            }
        }

        private CreateViewModel RecreateSingleViewModel(CreateViewModel vm, Game game, string errorMessage)
        {
            ModelState.Clear();
            vm.PlayerList = CreatePlayerList();
            vm.GameTypes = CreateTableTennisGameTypes(game);
            ModelState.AddModelError("ValidationError", errorMessage);

            return vm;
        }

        private CreateDoubleViewModel RecreateDoubleViewMdoel(CreateDoubleViewModel vm, Game game, string errorMessage)
        {
            ModelState.Clear();
            vm.PlayerList = CreatePlayerList();
            vm.GameTypes = CreateTableTennisGameTypes(game);
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
