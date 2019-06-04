using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class Order
    {
        [Display(Name = "Log Nr")]
        public int OrderID { get; set; }

        [Display(Name = "Vare Nr")]
        public string VareNr { get; set; }

        [Display(Name = "Produkt Navn")]
        public string ProduktNavn { get; set; }

        [Display(Name = "Antal Ind")]
        public int AntalInd { get; set; }

        [Display(Name = "Antal Ud")]
        public int AntalUd { get; set; }        

        [Display(Name = "Bruger navn")]
        public string UserID { get; set; }

        [Display(Name = "Fuldt Navn")]
        public string DeliveryName { get; set; }

        [Display(Name = "Dato og Tid")]
        public DateTime DateOgTid { get; set; }

        public List<KontaktListen> OrderLines { get; set; }
    }
}