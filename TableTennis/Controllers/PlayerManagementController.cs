using System;
using System.Web.Mvc;
using TableTennis.Interfaces.Repository;
using TableTennis.ViewModels;
using System.Linq;

namespace TableTennis.Controllers
{
    public class PlayerManagementController : Controller
    {
        private readonly IPlayerManagementRepository _playerManagementRepository;
        private readonly IMatchManagementRepository _matchManagementRepository;

        public PlayerManagementController(IPlayerManagementRepository playerManagementRepository, IMatchManagementRepository matchManagementRepository)
        {
            _playerManagementRepository = playerManagementRepository;
            _matchManagementRepository = matchManagementRepository;
        }

        //
        // GET: /UserManagement/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /UserManagement/Details/5

        public ActionResult Details(Guid id)
        {
            var vm = new PlayerDetailsViewModel
                {
                    Player = _playerManagementRepository.GetPlayerById(id)
                };

            return View(vm);
        }

        //
        // GET: /UserManagement/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PlayerList()
        {
            var playerList = _playerManagementRepository.GetAllPlayers();

            foreach (var player in playerList)
            {
                player.Rating = _matchManagementRepository.GetPlayerRatingByPlayerId(player.Id);
            }

            playerList = playerList.OrderByDescending(player => player.Rating).ToList();

            var viewModel = new PlayerListViewModel {PlayerList = playerList};
            
            return View(viewModel);
        }

        //
        // POST: /UserManagement/Create

        [HttpPost]
        public ActionResult Create(PlayerManagementViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _playerManagementRepository.CreatePlayer(viewModel.Player);
                    if (!result)
                    {
                        ViewBag["Error"] = "Player creating failed";
                    }

                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserManagement/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /UserManagement/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /UserManagement/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /UserManagement/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
