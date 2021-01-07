using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClothingShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatorie")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}