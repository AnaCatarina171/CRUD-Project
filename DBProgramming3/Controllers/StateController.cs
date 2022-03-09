using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult StateList()
        {
            var context = new TechSupportEntities();

            List<State> states = context.States.OrderBy(x => x.StateName).ToList();

            return View(states);
        }

        public ActionResult AddOrUpdateState()
        {
            var context = new TechSupportEntities();

            return View();
        }
    }
}