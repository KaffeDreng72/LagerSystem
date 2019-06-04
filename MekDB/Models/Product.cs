using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MekDB.Models
{
    public class Product
    {
        // Primary Key
        public int ID { get; set; }

        [Display(Name = "Vare Nr.")]
        public string VareNr { get; set; }

        [Display(Name = "Produkt Navn")]
        public string ProduktNavn { get; set; }

        [Display(Name = "Dansk Navn")]
        public string DanskNavn { get; set; }

        [Display(Name = "Lager Status")]
        public int LagerStatus { get; set; }

        [Display(Name = "Bestil Ved:")]
        public int BestilVed { get; set; }

        public string Bemærkning { get; set; }

        public byte[] Image { get; set; }

        // Foreign Key
        public int? LocationID { get; set; }

        // Navigation Property
        [Display(Name = "Lokation")]
        public virtual Location Location { get; set; }

        // Foreign Key
        public int? SupplierID { get; set; }

        // Navigation Property
        [Display(Name = "Leverandør")]
        public virtual Suppliers Suppliers { get; set; }
    }
}