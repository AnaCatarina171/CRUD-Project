using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult RegistrationList()
        {
            var context = new TechSupportEntities();

            List<Registration> registrations = context.Registrations.OrderBy(x => x.RegistrationDate).ToList();

            return View(registrations);
        }
    }
}