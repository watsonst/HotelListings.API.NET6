namespace HotelListings.API.Core.Models
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }//how many records in table
        public int PageNumber { get; set; }
        public int RecordNumber { get; set; }
        public List<T> Items { get; set; }
    }

}
