using System;
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

        //
        // GET: /Match/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Match/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Match/Create
        [HttpGet]
        public ActionResult Create()
        {
            var vm = new CreateMatchViewModel
                         {
                             Winner =
                                 CreateWinnerList(),
                             PlayerList = CreatePlayerList()
                         }
                ;

            return View(vm);
        }

        //
        // POST: /Match/Create

        [HttpPost]
        public ActionResult Create(CreateMatchViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Clear();
                    vm.PlayerList = CreatePlayerList();
                    vm.Winner = CreateWinnerList();
                    ModelState.AddModelError("ValidationError", "Failed to submit, invalid data!");

                    return View(vm);
                }
                if (vm.Player1ID == vm.Player2ID)
                {
                    ModelState.Clear();
                    vm.PlayerList = CreatePlayerList();
                    vm.Winner = CreateWinnerList();
                    ModelState.AddModelError("ValidationError", "Select different players!");
                    return View(vm);
                }

                var player1Rating = _playerManagementRepository.GetPlayerRatingById(vm.Player1ID);
                var player2Rating = _playerManagementRepository.GetPlayerRatingById(vm.Player2ID);

                var playerOneWin = vm.WinnerID == 1 ? 1 : 0;
                var playerTwoWin = vm.WinnerID == 2 ? 1 : 0;

                var ratingSystem = new EloRating(player1Rating, player2Rating, playerOneWin, playerTwoWin);

                var game = new PlayedGame
                               {
                                   EloPoints = playerOneWin == 1 ? (int) ratingSystem.Point1 : (int) ratingSystem.Point2,
                                   PlayerIds = {vm.Player1ID, vm.Player2ID},
                                   Ranked = true,
                                   TimeStamp = DateTime.UtcNow,
                                   WinnerId = playerOneWin == 1 ? vm.Player1ID : vm.Player2ID
                               };

                _playerManagementRepository.UpdateRating(vm.Player1ID, (int) ratingSystem.FinalResult1);
                _playerManagementRepository.UpdateRating(vm.Player2ID, (int) ratingSystem.FinalResult2);

                //Validate game score
                var errorMessage = "";
                var validationResult = ValidateMatch.ValidateGame(Game.TableTennis, GameType.Standard, game.GameSets, out errorMessage);

                if (!validationResult)
                {
                    ModelState.Clear();
                    vm.PlayerList = CreatePlayerList();
                    vm.Winner = CreateWinnerList();
                    ModelState.AddModelError("MatchValidationError", errorMessage);
                    return View(vm);
                }

                {game.GameSets.Add(new GameSet {Score1 = vm.Score1Set1, Score2 = vm.Score2Set1});
                if (vm.Score1Set2 != 0 || vm.Score2Set2 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set2, Score2 = vm.Score2Set2});
                }
                if (vm.Score1Set3 != 0 || vm.Score2Set3 != 0)
                {
                    game.GameSets.Add(new GameSet {Score1 = vm.Score1Set3, Score2 = vm.Score2Set3});
                }

                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerList", "PlayerManagement");
            }}
            catch
            {
                return View();
            }
        }

        private IEnumerable<SelectListItem> CreatePlayerList()
        {
            List<Player> playerList = _playerManagementRepository.GetAllPlayers();
            return playerList.Select(p => new SelectListItem
                                              {
                                                  Text = p.Username,
                                                  Value = p.Id.ToString()
                                              });
        }

        private SelectListItem[] CreateWinnerList()
        {
            return new[]
                       {
                           new SelectListItem {Text = "Player 1", Value = 1.ToString(), Selected = true}
                           , new SelectListItem {Text = "Player 2", Value = 2.ToString(), Selected = false}
                       };
        }

        //
        // GET: /Match/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Match/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Match/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Match/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}