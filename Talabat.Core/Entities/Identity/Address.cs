namespace Talabat.Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
        public string AppUserId { get; set; } // Foreign Key for AppUser [1 to 1]

        // public AppUser AppUser { get; set; } // when you add this don't forget to handle the Address in AccountController (make it AddressDto to avoid circular reference)

    }
}