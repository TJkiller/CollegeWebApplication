namespace CollegeWebApp.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string? StudentName { get; set; }

       
        public List<Module>? Modules { get; set; }
    }

}
