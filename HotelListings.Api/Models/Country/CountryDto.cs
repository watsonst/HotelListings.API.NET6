using HotelListings.Api.Models.Hotel;

namespace HotelListings.Api.Models.Country
{
    public class CountryDto //may not need to get as granular for dto's/Have as many dto's
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public List<HotelDto> Hotels { get; set; } //Dto should never have a field that is directly related to model type. So have to create HotelDto. 
    }

}
