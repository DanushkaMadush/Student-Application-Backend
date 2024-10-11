using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApplication.Data;
using StudentApplication.Models;
using StudentApplication.Models.Entities;

namespace StudentApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents() 
        {
            var allStudents = dbContext.Students.ToList();
            return Ok(allStudents);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchStudents(string searchTerm = null)
        {
            var students = await dbContext.Students.ToListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                students = students.Where(s => s.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.LastName.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.StudentEmail.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.Phone.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.Country.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.Address.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.Institute.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.Intake.ToLower().Contains(searchTerm.ToLower()) ||
                                                s.CourseTitle.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            return Ok(students);
        }

        [HttpGet("filterStudent")]
        public async Task<IActionResult> FilterStudents(string approval = null, string country = null, string institute = null, string intake = null)
        {
            var students = await dbContext.Students.ToListAsync();

            var approvalLower = approval?.ToLower();
            var countryLower = country?.ToLower();
            var instituteLower = institute?.ToLower();
            var intakeLower = intake?.ToLower();

            if (!string.IsNullOrEmpty(approvalLower))
            {
                students = students.Where(s => s.Approval?.ToLower() == approvalLower).ToList();
            }
            if (!string.IsNullOrEmpty(countryLower))
            {
                students = students.Where(s => s.Country?.ToLower() == countryLower).ToList();
            }
            if (!string.IsNullOrEmpty(instituteLower))
            {
                students = students.Where(s => s.Institute?.ToLower() == instituteLower).ToList();
            }
            if (!string.IsNullOrEmpty(intakeLower))
            {
                students = students.Where(s => s.Intake?.ToLower() == intakeLower).ToList();
            }

            return Ok(students);
        }



        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", fileName);
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] AddStudentDto addStudentDto)
        {
            var existingStudent = dbContext.Students.FirstOrDefault(s => s.StudentEmail == addStudentDto.StudentEmail);
            if (existingStudent != null)
            {
                return BadRequest("Email already exists.");
            }

            if (addStudentDto.File == null || addStudentDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Save the file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", addStudentDto.File.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await addStudentDto.File.CopyToAsync(stream);
            }

            var studentEntity = new Student()
            {
                FirstName = addStudentDto.FirstName,
                LastName = addStudentDto.LastName,
                StudentEmail = addStudentDto.StudentEmail,
                Phone = addStudentDto.Phone,
                Address = addStudentDto.Address,
                Country = addStudentDto.Country,
                Institute = addStudentDto.Institute,
                Intake = addStudentDto.Intake,
                CourseTitle = addStudentDto.CourseTitle,
                FilePath = filePath 
            };

            dbContext.Students.Add(studentEntity);
            dbContext.SaveChanges();

            return Ok(studentEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateStudent(Guid id, UpdateStudentDto updateStudentDto)
        {
            var student = dbContext.Students.Find(id);

            if (student is null)
            {
                return NotFound();
            }

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;
            student.StudentEmail = updateStudentDto.StudentEmail;
            student.Phone = updateStudentDto.Phone;
            student.Address = updateStudentDto.Address;
            student.Country = updateStudentDto.Country;
            student.Institute = updateStudentDto.Institute;
            student.Intake = updateStudentDto.Intake;
            student.CourseTitle = updateStudentDto.CourseTitle;
            student.License = updateStudentDto.License;
            student.Approval = updateStudentDto.Approval;
            student.ExpiryDate = updateStudentDto.ExpiryDate;

            dbContext.SaveChanges();
            return Ok(student);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteStudent(Guid id)
        {
            var student = dbContext.Students.Find(id);

            if (student is null)
            {
                return NotFound();
            }

            dbContext.Students.Remove(student);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
