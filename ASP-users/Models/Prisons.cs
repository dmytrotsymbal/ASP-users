namespace ASP_users.Models
{
    public class Prisons
    {
        public int PrisonID { get; set; }
        public string PrisonName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public SecurityLevelEnum? SecurityLevel { get; set; }

        public enum SecurityLevelEnum
        {
            Low,
            Medium,
            High,
            Maximum
        }
    }
}
