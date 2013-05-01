using System;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Create()
        {
            var playerList = _playerManagementRepository.GetAllPlayers();

            var vm = new CreateMatchViewModel
                {
                    Winner =
                        new[]
                            {
                                new SelectListItem {Text = "Player 1", Value = 1.ToString(), Selected = true}
                                , new SelectListItem {Text = "Player 2", Value = 2.ToString(), Selected = false}
                            },
                    PlayerList = playerList.Select(p => new SelectListItem
                        {
                            Text = p.Username,
                            Value = p.Id.ToString()
                        })
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
                    ViewBag["Error"] = "This is wrong!";
                    return View();
                }
                if (vm.Player1ID == vm.Player2ID)
                {
                    ViewBag["Error"] = "Select differnet players";
                    return View();
                }

                var player1Rating = _matchManagementRepository.GetPlayerRatingByPlayerId(vm.Player1ID);
                var player2Rating = _matchManagementRepository.GetPlayerRatingByPlayerId(vm.Player2ID);

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

                _matchManagementRepository.CreateMatch(game);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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