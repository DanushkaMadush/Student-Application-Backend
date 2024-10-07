﻿using Microsoft.AspNetCore.Http;
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
    }
}
