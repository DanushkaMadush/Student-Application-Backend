namespace StudentApplication.Models
{
    public class AddStudentDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string StudentEmail { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
        public required string Country { get; set; }
        public required string Institute { get; set; }
        public required string Intake { get; set; }
        public required string CourseTitle { get; set; }
        public IFormFile? File { get; set; }
    }
}
