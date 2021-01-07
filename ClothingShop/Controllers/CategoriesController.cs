using ClothingShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectBun.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var cat = from category in db.Categories
                      orderby category.Name
                      select category;
            ViewBag.Categories = cat;
            return View();
        }

        //Show
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //New
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost adaugata cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                return View(category);
            }
        }


        //Edit
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);

                    if (TryUpdateModel(category))
                    {
                        category.Name = requestCategory.Name;
                        db.SaveChanges();
                        TempData["message"] = "Categoria a fost modificata! cu succes";
                        return RedirectToAction("Index");
                    }

                    return View(requestCategory);
                }
                else
                {
                    return View(requestCategory);
                }
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        //Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            TempData["message"] = "Categoria a fost stearsa cu succes";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}