using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class IncidentController : Controller
    {
        // GET: Incident
        public ActionResult IncidentList()
        {
            var context = new TechSupportEntities();

            List<Incident> incidents = context.Incidents.OrderBy(x => x.IncidentID).ToList();

            return View(incidents);
        }
    }
}