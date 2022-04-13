using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class TechnicianController : Controller
    {
        // GET: Technician
        public ActionResult TechList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
        {
            var context = new TechSupportEntities();

            List<Technician> technicians = context.Technicians.OrderBy(x => x.Name).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    technicians = technicians
                        .Where(t => t.TechID.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    technicians = technicians
                        .Where(t => t.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 3)
                {
                    technicians = technicians
                        .Where(t => t.Phone.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 4)
                {
                    technicians = technicians
                        .Where(t => t.Email.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            int skip = (page - 1) * (top);
            int totalItems = technicians.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            technicians = technicians.Skip(skip).Take(top).ToList();

            return View(technicians);
        }


        [HttpPost]
        public ActionResult CreateTech(string txtTechName, string txtTechPhone, string txtTechEmail)
        {
            string message = "";

            try
            {
                if (txtTechName == "" || txtTechPhone == "" || txtTechEmail == "")
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    Technician technician = new Technician
                    {
                        Name = txtTechName,
                        Phone = txtTechPhone,
                        Email = txtTechEmail
                    };

                    AddOrUpdateTech(technician);

                    message = "The technician " + txtTechName + " was successfuly added.";
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

        public ActionResult AddOrUpdateTech(int id = 0)
        {
            var context = new TechSupportEntities();

            Technician technician = context.Technicians.FirstOrDefault(t => t.TechID == id);

            if (id == 0)
            {
                technician = new Technician();
            }

            return View(technician);
        }

        [HttpPost]
        public ActionResult AddOrUpdateTech(Technician technician)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/Technician/TechList";

            try
            {
                context.Technicians.AddOrUpdate(technician);
                context.SaveChanges();

                message = "Technician " + technician.Name + " was successfully updated.";
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

        public ActionResult Delete(int txtTechId)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/Technician/TechList";

            try
            {
                Technician techToRemove = context.Technicians.FirstOrDefault(t => t.TechID == txtTechId);

                context.Technicians.Remove(techToRemove);
                context.SaveChanges();

                message = "Technician " + techToRemove.Name + " was successfully deleted.";
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