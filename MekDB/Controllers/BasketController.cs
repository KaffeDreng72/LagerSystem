using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MekDB.Models;
using MekDB.ViewModels;

namespace MekDB.Controllers
{
    public class BasketController : Controller
    {
        // GET: Basket
        public ActionResult Index()
        {
            Basket basket = Basket.GetBasket();
            BasketViewModel viewModel = new BasketViewModel
            {
                BasketLines = basket.GetBasketLines(),
                
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToBasketAntalInd(int id, int antalInd)
        {
            int antalUd = 0;
            Basket basket = Basket.GetBasket();
            basket.AddToBasket(id, antalInd,antalUd);
            TempData["Tekst"] = "Antal Ind på Lager";
            TempData["Tal"] = antalInd;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToBasketAntalUd(int id, int antalUd)
        {
            int antalInd = 0;
            Basket basket = Basket.GetBasket();
            basket.AddToBasket(id, antalInd, antalUd);
            TempData["Tekst"] = "Antal ud fra Lager";
            TempData["Tal"] = antalUd;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBasket(BasketViewModel viewModel)
        {
            Basket basket = Basket.GetBasket();
            basket.UpdateBasket(viewModel.BasketLines);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult FortrydOrder(int id)
        {
            Basket basket = Basket.GetBasket();
            basket.EmptyBasket();
            return RedirectToAction("Index", "Products");
        }
    }
}