using System.Web.Mvc;
using TableTennis.Authentication.MongoDB;
using TableTennis.ViewModels;

namespace TableTennis.Controllers
{
    public class PlayerManagementController : Controller
    {
        private readonly IMongoPlayerManagement _mongoPlayerManagement;
        private readonly IMongoMatchManagement _mongoMatchManagement;

        public PlayerManagementController(IMongoPlayerManagement mongoPlayerManagement, IMongoMatchManagement mongoMatchManagement)
        {
            _mongoPlayerManagement = mongoPlayerManagement;
            _mongoMatchManagement = mongoMatchManagement;
        }

        //
        // GET: /UserManagement/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /UserManagement/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /UserManagement/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PlayerList()
        {
            var playerList = _mongoPlayerManagement.GetAllPlayers();

            foreach (var player in playerList)
            {
                player.Rating = _mongoMatchManagement.GetPlayerRatingByPlayerId(player.Id);
            }

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
                    var result = _mongoPlayerManagement.CreatePlayer(viewModel.Player);
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
