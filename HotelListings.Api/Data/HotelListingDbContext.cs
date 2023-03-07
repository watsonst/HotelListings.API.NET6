using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HotelListings.Api.Data
{
    public class HotelListingDbContext : DbContext // DbContext part of Entity library
    {
        public HotelListingDbContext(DbContextOptions options) : base(options) //options comming from the options in the program.cs
        {


        }

        public DbSet<Hotel> Hotels { get; set; } //create dbset/table for the Db of type Hotel during "add-migration InitialMigration"
        public DbSet<Country> Countries { get; set; } //create dbset/table for the Db of type Countries during "add-migration InitialMigration"

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Islands",
                    ShortName = "CI"
                }
            );


            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 3,
                    Rating = 4.3
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Pallidum",
                    Address = "Nassua",
                    CountryId = 2,
                    Rating = 4
                }
            );
        }
    }
}
