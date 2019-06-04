using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class Suppliers
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = ("Leverandør"))]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}