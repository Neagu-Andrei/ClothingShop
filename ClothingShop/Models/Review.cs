using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClothingShop.Models
{
    public class Review
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage = "Campul acesta nu poate fi necompletat")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Campul acesta nu poate fi necompletat")]
        [Range(0, 5, ErrorMessage = "Trebuie sa alegeti un numar intre 0 si 5")]
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Product Product { get; set; }
    }
}