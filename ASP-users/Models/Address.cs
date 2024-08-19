namespace ASP_users.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        public Guid UserID { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int PostalCode { get; set; }

        public string Country { get; set; }
    }
}
