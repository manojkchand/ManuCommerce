using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CoreSite1.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreSite1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ExtendedUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categorys { get; set; }

        public DbSet<Variant> Variants { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageCategory> PCategorys { get; set; }
        public DbSet<PageTemplate> PTemplate { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<OrderStatuses> OrderStatus { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }

        public DbSet<SizeGuide> SizeGuide { get; set; }
        public DbSet<ToDoItem> ToDoItem { get; set; }

        public DbSet<MapImage> MapImage { get; set; }
        public DbSet<MapStock> MapStock { get; set; }
    }
}
