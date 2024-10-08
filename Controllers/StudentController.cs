using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        }

        [HttpGet]
        public IActionResult GetAllStudents() 
        {
            var allStudents = dbContext.Students.ToList();
            return Ok(allStudents);
        }

        [HttpPost]
        public IActionResult AddStudent(AddStudentDto addStudentDto)
        {
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
