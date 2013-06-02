using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TableTennis.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;
using TableTennis.ViewModels;

namespace TableTennis.Controllers
{
    public class PlayerManagementController : Controller
    {
        private readonly IMatchManagementRepository _matchManagementRepository;
        private readonly IPlayerManagementRepository _playerManagementRepository;

        public PlayerManagementController(IPlayerManagementRepository playerManagementRepository,
                                          IMatchManagementRepository matchManagementRepository)
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

        public ActionResult Details(string username)
        {
            List<PlayedGame> playedGames = _matchManagementRepository.GetAllGamesByUsername(username);

            var playedGamesVM = new PlayedGamesViewModel(playedGames, username);

            var vm = new PlayerDetailsViewModel
                {
                    Player = _playerManagementRepository.GetPlayerByUsername(username),
                    PlayedGamesViewModel = playedGamesVM
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
            List<Player> tempPlayerList = _playerManagementRepository.GetAllPlayers();
            List<Player> playerList =
                tempPlayerList.Where(player => player.Ratings.ContainsKey(Game.SingleTableTennis))
                              .OrderByDescending(player => player.Ratings[Game.SingleTableTennis])
                              .ToList();

            var viewModel = new PlayerListViewModel { PlayerList = playerList };

            return View(viewModel);
        }

        public ActionResult PlayerListTTDouble()
        {
            List<Player> tempPlayerList = _playerManagementRepository.GetAllPlayers();
            List<Player> playerList =
                tempPlayerList.Where(player => player.Ratings.ContainsKey(Game.DoubleTableTennis))
                              .OrderByDescending(player => player.Ratings[Game.DoubleTableTennis])
                              .ToList();

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
                    bool result = _playerManagementRepository.CreatePlayer(viewModel.Player);
                    if (!result)
                    {
                        ModelState.AddModelError("ModelError", "Username is taken!");
                        return View(viewModel);
                    }

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("ModelError", "Model was not valid!");

                return View(viewModel);
            }
            catch
            {
                return View();
            }
        }
    }
}