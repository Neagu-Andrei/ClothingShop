using ClothingShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace ClothingShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            var search = Request.Params["search"];
            var sort = Request.Params["sort"];
            if (search == null)
                search = "";
            if (sort == null)
                sort = "";
            var products = db.Products.Include("Category").Include("User").Where(product => product.Title.Contains(search)).ToList(); ;
            foreach (var product in products)
                GetRating(product);
            if (sort == "price-asc")
                products = products.OrderBy(product => product.Price).ToList();
            if (sort == "price-desc")
                products = products.OrderByDescending(product => product.Price).ToList();
            if (sort == "rating-asc")
                products = products.OrderBy(product => product.ProductRating).ToList();
            if (sort == "rating-desc")
                products = products.OrderByDescending(product => product.ProductRating).ToList();

            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }



        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            SetAccessRights();
            GetRating(product);
            return View(product);
        }
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(Review review)
        {
            review.Date = DateTime.Now;
            review.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return Redirect("/Products/Show/" + review.ProductId);
                }
                else
                {
                    Product prod = db.Products.Find(review.ProductId);
                    SetAccessRights();
                    return View(prod);
                }
            }
            catch(Exception e)
            {
                Product prod = db.Products.Find(review.ProductId);
                SetAccessRights();
                return View(prod);
            }
        }

        [Authorize(Roles = "Contribuitor,Admin")]
        public ActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Contribuitor,Admin")]
        public ActionResult New(Product product, HttpPostedFileBase uploadedFile)
        {
            product.UserId = User.Identity.GetUserId();
            product.Categ = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    string uploadedFileName = uploadedFile.FileName;
                    string uploadedFileExtension = Path.GetExtension(uploadedFileName);
                    if (uploadedFileExtension == ".png" || uploadedFileExtension == ".jpg" || uploadedFileExtension == ".pdf")
                    {
                        string uploadFolderPath = Server.MapPath("~//Files//");

                        // 2. Se salveaza fisierul in acel folder
                        uploadedFile.SaveAs(uploadFolderPath + uploadedFileName);

                        // 3. Se face o instanta de model si se populeaza cu datele necesare
                        Models.File file = new Models.File
                        {
                            Extension = uploadedFileExtension,
                            FileName = uploadedFileName,
                            FilePath = uploadFolderPath + uploadedFileName
                        };
                        // 4. Se adauga modelul in baza de date
                        db.Files.Add(file);
                        db.SaveChanges();
                        product.FileId = file.FileId;
                    }
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["message"] = "Produsul a fost adaugat cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }
            }
            catch (Exception e)
            {
                return View(product);
            }
        }
        [Authorize(Roles = "Contribuitor,Admin")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            product.Categ = GetAllCategories();
            if(product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra acestui produs";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Contribuitor,Admin")]
        public ActionResult Edit(int id, Product requestProduct)
        {
            requestProduct.Categ = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);
                    if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                       
                        if (TryUpdateModel(product))
                        {
                            product = requestProduct;
                            db.SaveChanges();
                            TempData["message"] = "Produsul a fost modificat cu succes!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificariasupra acestui produs.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestProduct);
                }

            }
            catch (Exception e)
            {
                return View(requestProduct);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Contribuitor,Admin")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters cu succes!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest produs.";
                return RedirectToAction("Index");
            }
            
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categories = from cat in db.Categories
                             select cat;

            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        static public void GetRating(Product product)
        {
            var ratings = product.Reviews;
            var selectList = new List<int>();
            foreach (var rating in ratings)
            {
                selectList.Add(rating.Rating);
            }
            if (selectList.Count() != 0)
                product.ProductRating = selectList.Average();
            else
                product.ProductRating = 0;
        }
        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Contribuitor") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Approve()
        {
            var products = db.Products.Include("Category").Include("User");
            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int id)
        {
            Product product = db.Products.Find(id);
            product.Approved = true;
            if (TryUpdateModel(product))
            {
                db.SaveChanges();
                TempData["message"] = "Produsul a fost aprobat!";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}