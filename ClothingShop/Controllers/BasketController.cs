using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothingShop.Models;

namespace ClothingShop.Controllers
{
    [Authorize(Roles = "User,Editor,Admin")]
    public class BasketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Basket
        public ActionResult Index()
        {
            var products = new List<Product>();
            decimal totalPrice = 0;
            HttpCookie basket = Request.Cookies["Basket"];
            if(basket != null && !string.IsNullOrEmpty(basket.Value))
            {
                string[] split = basket.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string item in split)
                {
                    int productId = int.Parse(item);
                    var product = db.Products.Find(productId);
                    products.Add(product);
                    totalPrice += product.Price;
                }
            }
            ViewBag.Products = products;
            ViewBag.TotalPrice = totalPrice;
            return View();
        }

        public ActionResult New(int id)
        {
            HttpCookie basket = new HttpCookie("Basket");
            if (Request.Cookies["Basket"] != null)
                basket = Request.Cookies["Basket"];
            if (string.IsNullOrEmpty(basket.Value))
                basket.Value = id.ToString();
            else
                basket.Value += "," + id.ToString();
            basket.Expires = DateTime.Now.AddHours(24);
            Response.Cookies.Add(basket);
            return Redirect("/Basket");
        }

        public ActionResult Empty()
        {
            if(Request.Cookies["Basket"] != null)
            {
                HttpCookie basket = Request.Cookies["Basket"];
                basket.Value = "";
                basket.Expires = DateTime.Now;
                Response.Cookies.Add(basket);
            }
            return Redirect("/Basket");
        }
    }
}