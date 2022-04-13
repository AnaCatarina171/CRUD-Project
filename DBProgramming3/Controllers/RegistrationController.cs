using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult RegistrationList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
        {
            var context = new TechSupportEntities();

            List<Registration> registrations = context.Registrations.OrderBy(x => x.RegistrationDate).ToList();

            ViewBag.Customers = context.Customers.OrderBy(c => c.Name).ToList();
            ViewBag.Products = context.Products.OrderBy(p => p.Name).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    registrations = registrations
                        .Where(r => r.Product.ProductCode.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    registrations = registrations
                        .Where(r => r.Product.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 3)
                {
                    registrations = registrations
                        .Where(r => r.Customer.CustomerID.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 4)
                {
                    registrations = registrations
                        .Where(r => r.Customer.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            int skip = (page - 1) * (top);
            int totalItems = registrations.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            registrations = registrations.Skip(skip).Take(top).ToList();

            return View(registrations);
        }

        [HttpPost]
        public ActionResult CreateRegistration(string productListChoice, string customerListChoice, DateTime registrationDate)
        {
            string message = "";

            var context = new TechSupportEntities();

            try
            {
                Customer customer = context.Customers.FirstOrDefault(c => c.Name == customerListChoice);

                if (productListChoice == "" || customerListChoice == "" ||
                    registrationDate == null || customer == null)
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    int customerID = customer.CustomerID;

                    Registration registration = new Registration
                    {
                        ProductCode = productListChoice,
                        CustomerID = customerID,
                        RegistrationDate = registrationDate
                    };

                    AddOrUpdateRegistration(registration);

                    message = "A new registration was successfuly created.";
                }
            }
            catch (Exception ex)
            {
                message = "There is an error: " + ex.Message;
            }

            return new JsonResult()
            {
                Data = new { message },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public ActionResult AddOrUpdateRegistration(string productCode, int customerID)
        {
            var context = new TechSupportEntities();

            ViewBag.Customers = context.Customers.OrderBy(c => c.Name).ToList();
            ViewBag.Products = context.Products.OrderBy(p => p.Name).ToList();

            Registration registration = context.Registrations
                    .FirstOrDefault(r => r.ProductCode == productCode && r.CustomerID == customerID);

            if (productCode == "")
            {
                registration = new Registration();
            }

            return View(registration);
        }

        [HttpPost]
        public ActionResult AddOrUpdateRegistration(Registration registration)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/Registration/RegistrationList";

            try
            {
                context.Registrations.AddOrUpdate(registration);
                context.SaveChanges();

                message = "Registration was successfully updated.";

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

        public ActionResult Delete(string productCode, int customerID)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/Registration/RegistrationList";

            try
            {
                Registration registrationToRemove = context.Registrations
                    .FirstOrDefault(r => r.ProductCode == productCode && r.CustomerID == customerID);

                context.Registrations.Remove(registrationToRemove);
                context.SaveChanges();

                message = "Registration was successfully deleted.";
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

        public ActionResult elementRegistrations(string identification, string obj, string name)
        {
            var context = new TechSupportEntities();

            List<Registration> registrations = context.Registrations.OrderBy(x => x.RegistrationDate).ToList();

            ViewBag.Obj = obj;
            ViewBag.Name = name;
            ViewBag.ReturnLink = "";

            try
            {
                if (obj == "Product")
                {
                    registrations = registrations.Where(x => x.ProductCode == identification).ToList();

                    ViewBag.ReturnLink = "/Product/AddOrUpdateProduct?code=" + identification;
                }
                else
                {
                    int id = Convert.ToInt32(identification);

                    registrations = registrations.Where(x => x.CustomerID == id).ToList();

                    ViewBag.ReturnLink = "/Customer/AddOrUpdateCustomer/" + id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(registrations);
        }
    }
}