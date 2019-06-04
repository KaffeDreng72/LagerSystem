using MekDB.Models;
using System.Data.Entity;

namespace MekDB.DAL
{
    //controlls what to save in the database
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BasketLog> BasketLines { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
    }
}