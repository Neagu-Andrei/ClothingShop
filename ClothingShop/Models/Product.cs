using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothingShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul produsului este obligatoriu")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pretul produsului este obligatoriu")]
        [Range(1, 999.99)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }
        public int FileId { get; set; }
        public string UserId { get; set; }
        public bool Approved { get; set; }
        public double ProductRating { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual File File { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public IEnumerable<SelectListItem> Categ { get; set; }

    }
}