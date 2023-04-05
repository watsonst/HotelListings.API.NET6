namespace HotelListings.API.Core.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) : base($"{name} with id ({key}) was not found") //custom exception example. Use object key so it is generic enough to take any data type
        {
            
        }
    }
}
