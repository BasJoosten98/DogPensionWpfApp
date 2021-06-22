using DogPensionWpfApp.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.EntityFramework
{
    public class DogPensionDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }

        public DogPensionDbContext(DbContextOptions options) 
            : base(options)
        {

        }
    }
}
