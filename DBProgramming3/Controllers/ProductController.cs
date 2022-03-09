using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductList()
        {
            var context = new TechSupportEntities();

            List<Product> products = context.Products.OrderBy(x => x.Name).ToList();

            return View(products);
        }
    }
}