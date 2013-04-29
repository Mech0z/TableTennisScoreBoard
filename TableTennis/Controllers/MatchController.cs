using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableTennis.Authentication.MongoDB;
using TableTennis.ViewModels;

namespace TableTennis.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMongoPlayerManagement _mongoPlayerManagement;

        public MatchController()
        {
            _mongoPlayerManagement = new MongoPlayerManagement();
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
            var playerList = _mongoPlayerManagement.GetAllPlayers();

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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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