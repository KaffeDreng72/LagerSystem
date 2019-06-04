using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MekDB.Models
{
    public class BasketLog
    {
        public int ID { get; set; }
        public string BasketID { get; set; }
        public int ProductID { get; set; }        
        public int AntalInd { get; set; }
        public int AntalUd { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}