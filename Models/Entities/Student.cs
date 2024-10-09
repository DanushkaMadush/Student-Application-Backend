namespace StudentApplication.Models.Entities
{
    public class Student
    {
        public Guid StudentID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string StudentEmail { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
        public required string Country { get; set; }
        public required string Institute { get; set; }
        public required string Intake { get; set; }
        public required string CourseTitle { get; set; }
        public string License { get; set; } = "Inactive"; 
        public string Approval { get; set; } = "Pending"; 
        public string ExpiryDate { get; set; } = "N/A";
        public required string FilePath { get; set; }
    }
}
