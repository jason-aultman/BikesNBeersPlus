using BikesNBeersMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
        public DbSet<BikerInfo> BikerInfos { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Badges> Badges { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BikerInfo>(b =>
            {
                b.HasKey(k => k.Id);
            });
            builder.Entity<Stop>(b =>
            {
                b.HasKey(k => k.Id);
            }); 
            builder.Entity<Badges>(b =>
            {
                b.HasKey(k => k.Id);
            }); 
            builder.Entity<Trip>(b =>
            {
                b.HasKey(k => k.Id);
            });

        }
    }
   

    public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=tcp:bikesnbeers-sqlserver.database.windows.net,1433;Initial Catalog=BikesNBeersDb;Persist Security Info=False;User ID=bikesnbeersadmin;Password=bikesnbeers!123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new ApplicationDbContext(builder.Options);
        }
    }
}
