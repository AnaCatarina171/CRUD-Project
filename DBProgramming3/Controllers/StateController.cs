using DBProgramming3.Controllers;
using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using System.Windows.Forms;

namespace DBProgramming3.Views
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult StateList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
        {
            var context = new TechSupportEntities();

            List<State> states = context.States.OrderBy(x => x.StateName).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    states = states
                        .Where(s => s.StateCode.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    states = states
                        .Where(s => s.StateName.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 3)
                {
                    states = states
                        .Where(s => s.FirstZipCode.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 4)
                {
                    states = states
                        .Where(s => s.LastZipCode.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            int skip = (page - 1) * (top);
            int totalItems = states.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            states = states.Skip(skip).Take(top).ToList();


            return View(states);
        }

        [HttpPost]
        public ActionResult CreateState(string txtStateCode, string txtStateName, int txtStateFZipCode = 0, int txtStateLZipCode = 0)
        {
            string message = "";

            try
            {
                if(txtStateCode == "" || txtStateName == "" || txtStateFZipCode == 0 || txtStateLZipCode == 0)
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    State state = new State
                    {
                        StateCode = txtStateCode,
                        StateName = txtStateName,
                        FirstZipCode = txtStateFZipCode,
                        LastZipCode = txtStateLZipCode
                    };

                    AddOrUpdateState(state);

                    message = "The state " + txtStateName + " was successfuly added.";
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

        public ActionResult AddOrUpdateState(string code = "")
        {
            var context = new TechSupportEntities();

            State state = context.States.FirstOrDefault(s => s.StateCode == code);

            if(code == "")
            {
                state = new State();
            }

            return View(state);
        }

        [HttpPost]
        public ActionResult AddOrUpdateState(State state)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/State/StateList";

            try
            {
                context.States.AddOrUpdate(state);
                context.SaveChanges();

                message = "State of " + state.StateName + " was successfully updated.";
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

        public ActionResult Delete(string txtStateCode)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "/State/StateList";

            try
            {
                State stateToRemove = context.States.FirstOrDefault(s => s.StateCode == txtStateCode);

                context.States.Remove(stateToRemove);
                context.SaveChanges();

                message = "State of " + stateToRemove.StateName + " was deleted.";
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