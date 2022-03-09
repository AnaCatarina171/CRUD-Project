using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult CustomerList()
        {
            var context = new TechSupportEntities();

            List<Customer> curstomers = context.Customers.OrderBy(x => x.Name).ToList();

            return View(curstomers);
        }
    }
}