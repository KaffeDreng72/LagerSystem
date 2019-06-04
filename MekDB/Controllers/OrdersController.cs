using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MekDB.DAL;
using MekDB.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MekDB.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private StoreContext db = new StoreContext();
        private ApplicationDbContext user_db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Orders
        public ActionResult Index(string orderSearch, string startDate, string endDate)
        {
            var orders = db.Orders.OrderBy(o => o.DateOgTid).AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                orders = orders.Where(o => o.UserID == User.Identity.Name);
            }
                        
            if (!String.IsNullOrEmpty(orderSearch))
            {
                orders = orders.Where(p => p.VareNr.Contains(orderSearch) ||
                                              p.ProduktNavn.Contains(orderSearch) ||
                                              p.UserID.Contains(orderSearch) ||                                               
                                              p.DeliveryName.Contains(orderSearch));
            }
           
            DateTime parsedStartDate;            
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                orders = orders.Where(o => o.DateOgTid >= parsedStartDate);
            }

            DateTime parsedEndDate;
            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                parsedEndDate = parsedEndDate.AddDays(1);
                orders = orders.Where(o => o.DateOgTid <= parsedEndDate);
            }

            orders = orders.OrderByDescending(o => o.DateOgTid);

            return View(orders);
        }

        private void SendEmail(string userName, string vareNr, string produktName)
        {
            
            var MailListe = user_db.Users.Where(p => p.KontaktListen == "Ja");

            string smtpAddress = "mail.efif.dk";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFrom = "zbc-service@zbc.dk";
            string password = "sommer10";
            string subject = "Bestillings Besked fra Mekaniker Databasen";
            string body =
                "Hejsa " + "<br /> <br />" +
                
                "Lager beholdningen for <br />" +

                "Vare nummer : " + vareNr + "<br />" +

                "Produktnavn : " + produktName + "<br />" +
                "er meget lav." + "<br /> <br />" +

                "Vil du venligst bestille nogle flere hjem " + "<br /> <br />" +

                "Med Venlig Hilsen <br />" +

                "Mekaniker Databasen";


            //string emailTo = "zbcclol1@zbc.dk";

            foreach (var email in MailListe)
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(email.UserName);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
            }

        }

        // GET: Orders/Review
        public async Task<ActionResult> Review()
        {          

            Basket basket = Basket.GetBasket();
            Order order = new Order();
            order.UserID = User.Identity.Name;
            ApplicationUser user = await UserManager.FindByNameAsync(order.UserID);
            order.DeliveryName = user.Fornavn + " " + user.Efternavn;
           
            foreach (var item in basket.GetBasketLines())
            {
                
                order.VareNr = item.Product.VareNr;
                order.ProduktNavn = item.Product.ProduktNavn;
                order.AntalInd = item.AntalInd;
                order.AntalUd = item.AntalUd;              

                order.DateOgTid = DateTime.Now;
                item.Product.LagerStatus = item.Product.LagerStatus + order.AntalInd;
                item.Product.LagerStatus = item.Product.LagerStatus - order.AntalUd;
                db.Orders.Add(order);
                db.SaveChanges();

                int tal = item.Product.LagerStatus - item.AntalUd;
                if (tal <= item.Product.BestilVed)
                {
                    SendEmail(order.DeliveryName,order.VareNr,order.ProduktNavn);
                }
            }

            basket.EmptyBasket();
            return RedirectToAction("Index","Orders");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
