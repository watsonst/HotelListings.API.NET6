namespace HotelListings.API.Core.Models
{
    public class QueryParameters
    {
        private int _pageSize = 15; //dafault size set to 15 but user can change that in the public PageSize
        public int StartIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
