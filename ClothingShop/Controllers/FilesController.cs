using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ClothingShop.Models;
using File = ClothingShop.Models.File;

namespace ProiectBun.Controllers
{
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Files
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase uploadedFile)
        //{
        //    string uploadedFileName = uploadedFile.FileName;
        //    string uploadedFileExtension = Path.GetExtension(uploadedFileName);

        //    if (uploadedFileExtension == ".png" || uploadedFileExtension == ".jpg" || uploadedFileExtension == ".gif")
        //    {
        //        string uploadFolderPath = Server.MapPath("~//Files//");

        //        uploadedFile.SaveAs(uploadFolderPath + uploadedFileName);

        //            File file = new File();
        //            file.Extension = uploadedFileExtension;
        //            file.FileName = uploadedFileName;
        //            file.FilePath = uploadFolderPath + uploadedFileName;

        //            db.Files.Add(file);
        //            db.SaveChanges();
        //            ViewBag.FileId = file.FileId;
        //            return Redirect("/Products/New");
                
        //    }
        //    return View();
        //}
    }
}