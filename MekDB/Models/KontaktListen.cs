using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class KontaktListen
    {
        public int ID { get; set; }
        public bool Valgt { get; set; }
        public virtual Order Order { get; set; }
    }
}