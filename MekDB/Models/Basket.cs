using MekDB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class Basket
    {
        private string BasketID { get; set; }
        private const string BasketSessionKey = "BasketID";
        private StoreContext db = new StoreContext();   

    


        private string GetBasketID()
        {
            if (HttpContext.Current.Session[BasketSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[BasketSessionKey] =
                 HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempBasketID = Guid.NewGuid();
                    HttpContext.Current.Session[BasketSessionKey] = tempBasketID.ToString();
                }
            }
            return HttpContext.Current.Session[BasketSessionKey].ToString();
        }

        public static Basket GetBasket()
        {
            Basket basket = new Basket();
            basket.BasketID = basket.GetBasketID();
            return basket;
        }

        public void AddToBasket(int productID, int antalInd, int antalUd)
        {
            var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID && b.ProductID == productID);

            if (basketLine == null)
            {
                basketLine = new BasketLog
                {
                    ProductID = productID,
                    BasketID = BasketID,
                    AntalInd = antalInd,
                    AntalUd = antalUd,
                    DateCreated = DateTime.Now
                };
                db.BasketLines.Add(basketLine);
            }
           
            db.SaveChanges();
        }

        public void RemoveLine(int productID)
        {
            var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID && b.ProductID
             == productID);
            if (basketLine != null)
            {
                db.BasketLines.Remove(basketLine);
            }
            db.SaveChanges();
        }

        public void UpdateBasket(List<BasketLog> lines)
        {
            foreach (var line in lines)
            {
                var basketLine = db.BasketLines.FirstOrDefault(b => b.BasketID == BasketID &&
                 b.ProductID == line.ProductID);
                if (basketLine != null)
                {
                   
                        RemoveLine(line.ProductID);
                   
                }
            }
            db.SaveChanges();
        }

        public void EmptyBasket()
        {
            var basketLines = db.BasketLines.Where(b => b.BasketID == BasketID);
            foreach (var basketLine in basketLines)
            {
                db.BasketLines.Remove(basketLine);
            }
            db.SaveChanges();
        }

        public List<BasketLog> GetBasketLines()
        {
            return db.BasketLines.Where(b => b.BasketID == BasketID).ToList();
        }

        
        public int GetNumberOfItems()
        {
            int numberOfItems = 0;
            if (GetBasketLines().Count > 0)
            {
                numberOfItems = db.BasketLines.Where(b => b.BasketID == BasketID).Sum(b => b.Product.LagerStatus);
            }

            return numberOfItems;
        }

        public void MigrateBasket(string userName)
        {
            //find the current basket and store it in memory using ToList()
            var basket = db.BasketLines.Where(b => b.BasketID == BasketID).ToList();

            //find if the user already has a basket or not and store it in memory using ToList()
            var usersBasket = db.BasketLines.Where(b => b.BasketID == userName).ToList();

            //if the user has a basket then add the current items to it
            if (usersBasket != null)
            {
                //set the basketID to the username
                string prevID = BasketID;
                BasketID = userName;
                //add the lines in anonymous basket to the user's basket
               
                //delete the lines in the anonymous basket from the database
                BasketID = prevID;
                EmptyBasket();
            }
            else
            {
                //if the user does not have a basket then just migrate this one
                foreach (var basketLine in basket)
                {
                    basketLine.BasketID = userName;
                }

                db.SaveChanges();
            }
            HttpContext.Current.Session[BasketSessionKey] = userName;
        }
    }
}