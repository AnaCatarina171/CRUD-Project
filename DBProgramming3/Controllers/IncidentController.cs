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
        public ActionResult IncidentList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
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

            int skip = (page - 1) * (top);
            int totalItems = incidents.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            incidents = incidents.Skip(skip).Take(top).ToList();

            return View(incidents);
        }

        public ActionResult CreateIncident(string txtIncTitle, string productListChoice, 
            DateTime txtIncDateOpened, string txtIncDescription, DateTime? txtIncDateClosed,
            int customerListChoice = 0, int technicianListChoice = 0)
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

            string message = "";

            string redirectUrl = "/Incident/IncidentList";

            try
            {
                context.Incidents.AddOrUpdate(incident);
                context.SaveChanges();

                message = "Incident #" + incident.IncidentID + " was successfully updated.";
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

        public ActionResult Delete(int txtIncidentId)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/Incident/IncidentList";

            try
            {
                Incident incidentToRemove = context.Incidents.FirstOrDefault(i => i.IncidentID == txtIncidentId);

                context.Incidents.Remove(incidentToRemove);
                context.SaveChanges();

                message = "Incident #" + incidentToRemove.IncidentID + " was successfully deleted.";
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

        public ActionResult elementIncidents(string identification, string obj, string name)
        {
            var context = new TechSupportEntities();

            List<Incident> incidents = context.Incidents.OrderBy(x => x.IncidentID).ToList();

            ViewBag.Obj = obj;
            ViewBag.Name = name;
            ViewBag.ReturnLink = "";

            try
            {
                if (obj == "Product")
                {
                    incidents = incidents.Where(x => x.ProductCode == identification).ToList();

                    ViewBag.ReturnLink = "/Product/AddOrUpdateProduct?code=" + identification;
                }
                else
                {
                    int id = Convert.ToInt32(identification);

                    if (obj == "Customer")
                    {
                        incidents = incidents.Where(x => x.CustomerID == id).ToList();

                        ViewBag.ReturnLink = "/Customer/AddOrUpdateCustomer/" + id;
                    }
                    else if (obj == "Technician")
                    {
                        incidents = incidents.Where(x => x.TechID == id).ToList();

                        ViewBag.ReturnLink = "/Technician/AddOrUpdateTech/" + id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(incidents);
        }
    }
}