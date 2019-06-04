using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class Location
    {
        public int ID { get; set; }
        // Dataannotations  se side 70 i bogen
        //[Required]
        //[StringLength(50, MinimumLength = 3)]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [Display(Name = "Lokation")]
        public string LokationName { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}