using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class IncidentController : Controller
    {
        // GET: Incident
        public ActionResult IncidentList(string cmbSearch, string searchTerm)
        {
            var context = new TechSupportEntities();

            List<Incident> incidents = context.Incidents.OrderBy(x => x.IncidentID).ToList();

            ViewBag.Customers = context.Customers.ToList();
            ViewBag.Technicians = context.Technicians.ToList();
            ViewBag.Products = context.Products.ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    incidents = incidents
                        .Where(i => i.IncidentID.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    incidents = incidents
                        .Where(i => i.Title.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 3)
                {
                    incidents = incidents
                        .Where(i => i.DateOpened.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 4)
                {
                    incidents = incidents
                        .Where(i => i.DateClosed.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 5)
                {
                    incidents = incidents
                        .Where(i => i.Customer.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 6)
                {
                    incidents = incidents
                        .Where(i => i.Technician.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 7)
                {
                    incidents = incidents
                        .Where(i => i.Product.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            return View(incidents);
        }

        public ActionResult CreateIncident(string txtIncTitle, int customerListChoice,
            int technicianListChoice, string productListChoice, DateTime txtIncDateOpened,
             string txtIncDescription, DateTime? txtIncDateClosed = null)
        {
            var context = new TechSupportEntities();
            
            string message = "";

            try
            {
                if (txtIncTitle == "" || customerListChoice == 0 || technicianListChoice == 0 ||
                    productListChoice == "" || txtIncDateOpened == null || txtIncDescription == "")
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    Incident incident = new Incident
                    {
                        Title = txtIncTitle,
                        CustomerID = customerListChoice,
                        TechID = technicianListChoice,
                        ProductCode = productListChoice,
                        DateOpened = txtIncDateOpened,
                        Description = txtIncDescription
                    };

                    if (!(txtIncDateClosed == null))
                    {
                        incident.DateClosed = txtIncDateClosed;
                    };

                    AddOrUpdateIncident(incident);

                    message = "The new incident was successfuly added.";
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

        public ActionResult AddOrUpdateIncident(int id = 0)
        {
            var context = new TechSupportEntities();

            ViewBag.Customers = context.Customers.ToList();
            ViewBag.Technicians = context.Technicians.ToList();
            ViewBag.Products = context.Products.ToList();

            Incident incident = context.Incidents.FirstOrDefault(i => i.IncidentID == id);

            if (id == 0)
            {
                incident = new Incident();
            }

            return View(incident);
        }

        [HttpPost]
        public ActionResult AddOrUpdateIncident(Incident incident)
        {
            var context = new TechSupportEntities();

            try
            {
                context.Incidents.AddOrUpdate(incident);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Redirect("/Incident/IncidentList");
        }

        public ActionResult Delete(int id)
        {
            var context = new TechSupportEntities();

            string message = "";

            try
            {
                Incident incidentToRemove = context.Incidents.FirstOrDefault(i => i.IncidentID == id);

                context.Incidents.Remove(incidentToRemove);
                context.SaveChanges();

                message = "Incident #" + incidentToRemove.IncidentID + " was deleted.";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }

            return Redirect("/Incident/IncidentList");
        }
    }
}