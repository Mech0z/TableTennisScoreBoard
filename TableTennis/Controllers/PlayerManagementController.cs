using System.Web.Mvc;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;
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

        public ActionResult Details(string username)
        {
            var playedGames = _matchManagementRepository.GetAllGamesByUsername(username);

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
            var playerList = _playerManagementRepository.GetAllPlayers();

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
                        ModelState.AddModelError("ModelError" ,"Username is taken!");
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
