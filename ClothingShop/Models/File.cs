using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothingShop.Models
{
    public class File
    {
        [Key]
        public int FileId { get; set; } //ID Primary Key

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}