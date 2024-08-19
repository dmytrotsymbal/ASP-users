namespace ASP_users.Models
{
    public class Photo
    {
        public int ImageID { get; set; }
       
        public Guid UserID { get; set; }

        public string ImageURL { get; set; }

        public string AltText { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
