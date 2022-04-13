using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
        {
            var context = new TechSupportEntities();

            List<Product> products = context.Products.OrderBy(x => x.Name).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    products = products
                        .Where(p => p.ProductCode.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    products = products
                        .Where(s => s.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            int skip = (page - 1) * (top);
            int totalItems = products.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            products = products.Skip(skip).Take(top).ToList();

            return View(products);
        }

        public ActionResult CreateProduct(string txtProdCode, string txtProdName, 
            decimal txtProdVersion, DateTime prodReleaseDate)
        {
            var context = new TechSupportEntities();

            string message = "";

            try
            {
                if (txtProdCode == "" || txtProdName == "" || prodReleaseDate == null)
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    Product product = new Product
                    {
                        ProductCode = txtProdCode,
                        Name = txtProdName,
                        Version = txtProdVersion,
                        ReleaseDate = prodReleaseDate
                    };

                    AddOrUpdateProduct(product);

                    message = "The product " + txtProdName + " was successfuly added.";
                }
            }
            catch (Exception ex)
            {
                message = "An error ocurred: " + ex.Message;
            }

            return new JsonResult()
            {
                Data = new { message },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public ActionResult AddOrUpdateProduct(string code = "")
        {
            var context = new TechSupportEntities();

            Product product = context.Products.FirstOrDefault(p => p.ProductCode == code);

            if (code == "")
            {
                product = new Product();
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult AddOrUpdateProduct(Product product)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "https://localhost:44323/Product/ProductList";

            try
            {
                context.Products.AddOrUpdate(product);
                context.SaveChanges();

                message = "Product " + product.Name + " was successfully updated.";
            }
            catch (Exception ex)
            {
                message = "An error occurred: " + ex.Message;
            }

            return new JsonResult()
            {
                Data = new { message, redirectUrl },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public ActionResult Delete(string txtProducCode)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "https://localhost:44323/Product/ProductList";

            try
            {
                Product productToRemove = context.Products.FirstOrDefault(p => p.ProductCode == txtProducCode);

                context.Products.Remove(productToRemove);
                context.SaveChanges();

                message = "Product " + productToRemove.Name + " was deleted.";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }

            return new JsonResult()
            {
                Data = new { message, redirectUrl },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }
    }
}