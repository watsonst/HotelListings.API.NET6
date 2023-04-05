using System.ComponentModel.DataAnnotations;

namespace HotelListings.API.Core.Models.Hotel
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public double? Rating { get; set; }

        [Required]
        [Range(1, int.MaxValue)] //set different validation rules
        public int CountryId { get; set; }
    }
}
