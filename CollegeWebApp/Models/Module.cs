namespace CollegeWebApp.Models
{
    public class Module
    {
        public int ModuleID { get; set; }

        public int StudentID { get; set; }
    
        public Student? Student { get; set; }

        
        public int VenueID { get; set; }
       
        public Venue? Venue { get; set; }

        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
    }

}
