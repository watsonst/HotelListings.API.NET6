using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListings.Api.Data
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }


        [ForeignKey(nameof(CountryId))] //String representation of a field -using nameof keeps it strongly typed instead of "CountryID"
        public int CountryId { get; set; }
        public Country Country { get; set; }


    }
}
