namespace CollegeWebApp.Models
{
    public class Venue
    {
        public int VenueID { get; set; }
        public string? VenueName { get; set; }
        public int? VenueNumber { get; set; }

      
        public List<Module>? Modules { get; set; }
    }

}
