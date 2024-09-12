namespace ASP_users.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        public Guid UserID { get; set; }

        public string StreetAddress { get; set; }

        public int HouseNumber { get; set; }

        public int ApartmentNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
