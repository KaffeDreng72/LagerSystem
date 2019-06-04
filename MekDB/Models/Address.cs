using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MekDB.Models
{
    public class Address
    {
        //WARNING: Renaming any of these variables wiil cause the database to become corrupt!
        public string Vej { get; set; }
        
        public string By { get; set; }
      
        public int PostNr { get; set; }

        [Display(Name = "Telefon Nummer")]
        public int TelefonNr { get; set; }
    }
}