using Microsoft.AspNetCore.Identity;

namespace HotelListings.Api.Data
{
    public class ApiUser : IdentityUser //Since extending IdentityUser with ApiUser, use ApiUser anywhere IU would be used in config
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
