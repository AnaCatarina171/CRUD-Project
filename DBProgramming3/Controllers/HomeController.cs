using DBProgramming3.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            /*
            string SenderEmail = ConfigurationManager.AppSettings.Get("SenderEmail");
            string SenderEmailPassword = ConfigurationManager.AppSettings.Get("SenderEmailPassword");
            string EmailSMTPServer = ConfigurationManager.AppSettings.Get("EmailSMTPServer");
            int EmailSMTPServerPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("EmailSMTPServerPort"));

            var client = new SmtpClient(EmailSMTPServer, EmailSMTPServerPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(SenderEmail, SenderEmailPassword),
                Timeout = 20000
            };

            client.Send(SenderEmail, "casilva2109@gmail.com", "test", "testbody");
            */
            /*
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(SenderEmail);
                mail.Subject = "Hello World";
                mail.Body = "<h1>Hello</h1>";
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient(EmailSMTPServer, EmailSMTPServerPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(SenderEmail, SenderEmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            */
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}