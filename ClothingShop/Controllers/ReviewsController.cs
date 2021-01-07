using ClothingShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectBun.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reviews

        public ActionResult Index(int id)
        {
            return View();
        }

        //Get: Delete
        [HttpDelete]
        [Authorize(Roles ="Admin,Contribuitor,User")]
        public ActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);
            if(rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
                return Redirect("/Products/Show/" + rev.ProductId);
            }
            else
            {
                TempData["message"] = "Nu puteti sa editati acest review deoarece nu va apartine";
                return Redirect("/Products/Show/" + rev.ProductId);
            }
            
        }

        //[HttpPost]
        //[Authorize(Roles ="Admin,Contribuitor,User")]
        //public ActionResult New(Review rev)
        //{
        //    rev.UserId = User.Identity.GetUserId();
        //    rev.Date = DateTime.Now;
        //    try
        //    {
        //        db.Reviews.Add(rev);
        //        db.SaveChanges();
        //        return Redirect("/Products/Show/" + rev.ProductId);
        //    }

        //    catch (Exception e)
        //    {
        //        return Redirect("/Products/Show/" + rev.ProductId);
        //    }
        //}
        //GET: New
        [Authorize(Roles = "Admin,Contribuitor,User")]
        public ActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(review);
            }
            else
            {
                TempData["message"] = "Nu puteti sa editati acest review deoarece nu va apartine";
                return RedirectToAction("Index", "Products");
            }
                
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Contribuitor,User")]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                    Review review = db.Reviews.Find(id);
                    if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(review))
                        {

                            review.Comment = requestReview.Comment;
                            review.Rating = requestReview.Rating;
                            review.Date = DateTime.Now;
                            db.SaveChanges();
                        }
                        return Redirect("/Products/Show/" + review.ProductId);
                    }
                    else
                    {
                        TempData["message"] = "Nu puteti sa editati acest review deoarece nu va apartine";
                        return RedirectToAction("Index", "Products");
                    }
            }
            catch (Exception e)
            {
                return View(requestReview);
            }
        }
    }

}