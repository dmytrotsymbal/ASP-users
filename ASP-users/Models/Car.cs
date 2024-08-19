using System.Numerics;

namespace ASP_users.Models
{
    public class Car
    {
        public Int128 CarID { get; set; }

        public Guid UserID { get; set; }

        public string Firm { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public int Year { get; set; }

        public string LicensePlate { get; set; }

        public string? CarPhotoURL { get; set; }
    }
}
