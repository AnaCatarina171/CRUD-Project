using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class TechnicianController : Controller
    {
        // GET: Technician
        public ActionResult TechList()
        {
            var context = new TechSupportEntities();

            List<Technician> technicians = context.Technicians.OrderBy(x => x.Name).ToList();

            return View(technicians);
        }
    }
}