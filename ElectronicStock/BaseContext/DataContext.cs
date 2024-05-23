﻿using ElectronicStock.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.BaseContext
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<DeliveryMethodProduct> DeliveryMethodProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ShopCard> ShopCards { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<User> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Stock;Trusted_Connection=True;Encrypt=False;");
        }
    }
}
