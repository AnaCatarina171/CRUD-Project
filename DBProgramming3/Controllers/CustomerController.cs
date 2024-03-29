﻿using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult CustomerList(string cmbSearch, string searchTerm, int top = 15, int page = 1)
        {
            var context = new TechSupportEntities();

            List<Customer> customer = context.Customers.OrderBy(x => x.Name).ToList();

            ViewBag.States = context.States.OrderBy(s => s.StateName).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();


                int value = Convert.ToInt32(cmbSearch);

                if (value == 1)
                {
                    customer = customer
                        .Where(c => c.CustomerID.ToString().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 2 || value == 0)
                {
                    customer = customer
                        .Where(c => c.Name.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 3)
                {
                    customer = customer
                        .Where(c => c.Phone.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 4)
                {
                    customer = customer
                        .Where(c => c.Email.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
                if (value == 5)
                {
                    customer = customer
                        .Where(c => c.State.ToLower().IndexOf(searchTerm) != -1)
                        .ToList();
                }
            }

            int skip = (page - 1) * (top);
            int totalItems = customer.Count();

            ViewBag.totalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;
            ViewBag.searchTerm = searchTerm;
            ViewBag.cmbSearch = cmbSearch;

            customer = customer.Skip(skip).Take(top).ToList();

            return View(customer);
        }

        public ActionResult CreateCustomer(string txtCustomerName, string txtCustomerPhone,
            string txtCustomerEmail, string customerStateChoice, string txtCustomerCity, 
            string txtCustomerAddress, string txtCustomerZipCode)
        {
            var context = new TechSupportEntities();

            string message = "";

            try
            {
                if (txtCustomerName == "" || txtCustomerPhone == "" || txtCustomerEmail == "" ||
                    customerStateChoice == null || txtCustomerCity == "" || txtCustomerAddress == "" ||
                    txtCustomerZipCode == "")
                {
                    message = "Please, provide all the necessary information.";
                }
                else
                {
                    Customer customer = new Customer
                    {
                        Name = txtCustomerName,
                        Phone = txtCustomerPhone,
                        Email = txtCustomerEmail,
                        State = customerStateChoice,
                        City = txtCustomerCity,
                        Address = txtCustomerAddress,
                        ZipCode = txtCustomerZipCode
                    };

                    AddOrUpdateCustomer(customer);

                    message = "The customer " + txtCustomerName + " was successfuly added.";
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

        public ActionResult AddOrUpdateCustomer(int id = 0)
        {
            var context = new TechSupportEntities();

            Customer customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);

            if (id == 0)
            {
                customer = new Customer();
            }

            ViewBag.States = context.States.OrderBy(s => s.StateName).ToList();

            return View(customer);
        }

        [HttpPost]
        public ActionResult AddOrUpdateCustomer(Customer customer)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "https://localhost:44323/Customer/CustomerList";

            try
            {
                context.Customers.AddOrUpdate(customer);
                context.SaveChanges();

                message = "Details for customer " + customer.Name + " were updated.";
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

        public ActionResult Delete(int txtCustomerId)
        {
            var context = new TechSupportEntities();

            string message = "";

            string redirectUrl = "https://localhost:44323/Customer/CustomerList";

            try
            {
                Customer customerToRemove = context.Customers.FirstOrDefault(c => c.CustomerID == txtCustomerId);

                context.Customers.Remove(customerToRemove);
                context.SaveChanges();

                message = "Customer " + customerToRemove.Name + " was deleted.";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }

            return new JsonResult() {
                Data = new { message, redirectUrl },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }
    }
}