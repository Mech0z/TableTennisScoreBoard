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
                if (vm.Player1Username == vm.Player2Username)
                {
                    ModelState.Clear();
                    vm.PlayerList = CreatePlayerList();
                    vm.Winner = CreateWinnerList();
                    ModelState.AddModelError("ValidationError", "Select different players!");
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
                var errorMessage = "";
                var validationResult = ValidateMatch.ValidateGame(Game.TableTennis, GameType.Standard, game.GameSets,
                                                                  out errorMessage);
                if (validationResult == -1)
                {
                    ModelState.Clear();
                    vm.PlayerList = CreatePlayerList();
                    vm.Winner = CreateWinnerList();
                    ModelState.AddModelError("MatchValidationError", errorMessage);
                    return View(vm);
                }

                var player1Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player1Username);
                var player2Rating = _playerManagementRepository.GetPlayerRatingByUsername(vm.Player2Username);

                var playerOneWin = validationResult == 1 ? 1 : 0;
                var playerTwoWin = validationResult == 2 ? 1 : 0;

                var ratingSystem = new EloRating(player1Rating, player2Rating, playerOneWin, playerTwoWin);

                game.EloPoints = playerOneWin == 1 ? (int) ratingSystem.Point1 : (int) ratingSystem.Point2;

                _playerManagementRepository.UpdateRating(vm.Player1Username, (int) ratingSystem.FinalResult1);
                _playerManagementRepository.UpdateRating(vm.Player2Username, (int) ratingSystem.FinalResult2);

                game.WinnerUsername = validationResult == 1 ? vm.Player1Username : vm.Player2Username;

                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("PlayerList", "PlayerManagement");
            }
            catch
            {
                return View();
            }
        }

        private IEnumerable<SelectListItem> CreatePlayerList()
        {
            var playerList = _playerManagementRepository.GetAllPlayers();
            return playerList.Select(p => new SelectListItem
                                              {
                                                  Text = p.Username,
                                                  Value = p.Username
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