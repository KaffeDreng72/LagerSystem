using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Configuration;
using MekDB.DAL;
using MekDB.Models;
using MekDB.ViewModels;
using System.Data.Entity.Migrations;

namespace MekDB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        [AllowAnonymous]
        public ActionResult Index(string search)
        {

            if (String.IsNullOrEmpty(search))
            {
                return View();;
            }

            Basket basket = Basket.GetBasket();
            basket.EmptyBasket();
            
            var products = db.Products.Include(p => p.Location);
            products = getProductsWithSuppliers(products); //add supliers to the products

            if (!String.IsNullOrEmpty(search))
            {
                                
                products = products.Where(
                                          p => p.VareNr.ToLower().Contains(search) ||
                                          p.ProduktNavn.ToLower().Contains(search) ||
                                          p.DanskNavn.ToLower().Contains(search) ||
                                          p.Location.LokationName.ToLower().Contains(search) ||
                                          p.Suppliers.Name.ToLower().Contains(search)
                                          );
               
            }

            return View(products);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.Suppliers = db.Suppliers.Find(product.SupplierID);

            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.LokationList = new SelectList(db.Locations, "ID", "LokationName");
            viewModel.SupplierList = new SelectList(db.Suppliers, "ID", "Name");

            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {

            // converts image to byte array
            byte[] uploadedImageFile;

            //check if the user uploads anything
            ProcessImage imageProsser = new ProcessImage();//validate the image
            if (viewModel.Image != null && viewModel.Image.ContentLength > 0 && imageProsser.ValidateImage(viewModel.Image, true))
            {
                //if the image is valid processs it
                try
                {
                    if (Boolean.Parse(ConfigurationManager.AppSettings["OptimizeImage"]))
                    {
                        uploadedImageFile = imageProsser.ConvertImageToJpg(viewModel.Image);
                    }
                    else
                    {
                        uploadedImageFile = new byte[viewModel.Image.InputStream.Length];
                        viewModel.Image.InputStream.Read(uploadedImageFile, 0, uploadedImageFile.Length);
                    }
                }
                catch (ConfigurationException cex)
                {
                    System.Diagnostics.Debug.WriteLine("####Invalid AppSettings Value" + cex.Message);
                    throw cex;
                }
                catch (FormatException fex)
                {
                    throw fex;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                uploadedImageFile = null;
            }

            Product product = new Product();
            product.VareNr = viewModel.VareNr;
            product.ProduktNavn = viewModel.ProduktNavn;
            product.DanskNavn = viewModel.DanskNavn;
            product.LagerStatus = viewModel.LagerStatus;
            product.BestilVed = viewModel.BestilVed;
            product.Bemærkning = viewModel.Bemærkning;
            product.Image = uploadedImageFile;
            product.LocationID = viewModel.LokationID;
            product.SupplierID = viewModel.SupplierID;

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.LokationList = new SelectList(db.Locations, "ID", "LokationName", product.LocationID);
            viewModel.SupplierList = new SelectList(db.Suppliers, "ID", "Name", product.SupplierID);

            return View(viewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel viewModel = new ProductViewModel();
            viewModel.LokationList = new SelectList(db.Locations, "ID", "LokationName", product.LocationID);
            viewModel.SupplierList = new SelectList(db.Suppliers, "ID", "Name", product.SupplierID);

            viewModel.ID = product.ID;
            viewModel.VareNr = product.VareNr;
            viewModel.ProduktNavn = product.ProduktNavn;
            viewModel.DanskNavn = product.DanskNavn;
            viewModel.LagerStatus = product.LagerStatus;
            viewModel.BestilVed = product.BestilVed;
            viewModel.Bemærkning = product.Bemærkning;
            viewModel.LokationID = product.LocationID;
            viewModel.SupplierID = product.SupplierID;

            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedImageFile;
                Product product = new Product();

                //check if the user uploads anything
                ProcessImage imageProsser = new ProcessImage();//validate the image
                if (viewModel.Image != null && viewModel.Image.ContentLength > 0 && imageProsser.ValidateImage(viewModel.Image, true))
                {
                    //if the image is valid processs it
                    try
                    {
                        if (Boolean.Parse(ConfigurationManager.AppSettings["OptimizeImage"]))
                        {
                            uploadedImageFile = imageProsser.ConvertImageToJpg(viewModel.Image);
                            product.Image = uploadedImageFile;
                        }
                    }
                    catch (ConfigurationException cex)
                    {
                        System.Diagnostics.Debug.WriteLine("####Invalid AppSettings Value" + cex.Message);
                        throw cex;
                    }
                    catch (FormatException fex)
                    {
                        throw fex;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
                else //no new image uploaded, use the old one
                {
                    product.Image = GetImage(viewModel.ID).FileContents;
                }

                product.ID = viewModel.ID;
                product.VareNr = viewModel.VareNr;
                product.ProduktNavn = viewModel.ProduktNavn;
                product.DanskNavn = viewModel.DanskNavn;
                product.LagerStatus = viewModel.LagerStatus;
                product.BestilVed = viewModel.BestilVed;
                product.Bemærkning = viewModel.Bemærkning;
                product.LocationID = viewModel.LokationID;
                product.SupplierID = viewModel.SupplierID;

                db.Set<Product>().AddOrUpdate(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.Suppliers = db.Suppliers.Find(product.SupplierID);

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private IQueryable<Product> getProductsWithSuppliers(IQueryable<Product> productsQue)
        {
            //convert the produckt que into someting you can actually work with
            List<Product> productsList = productsQue.ToList();

            //loop all products and only save the ones with matching suppliers
            for (int i = 0; i < productsQue.Count(); i++)
            {
                //since suppliers are a separete element, we need to find them first using their id stored in the product
                productsList.ElementAt(i).Suppliers = db.Suppliers.Find(productsList.ElementAt(i).SupplierID);
            }

            //then we cast our results back to saomething ASP can eat
           return productsList.AsQueryable();
        }

        public FileContentResult GetImage(int id)
        {
            // Converts varbinary to byte array with MIME type jpeg
            byte[] byteArray = db.Products.Find(id).Image;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }
    }
}
