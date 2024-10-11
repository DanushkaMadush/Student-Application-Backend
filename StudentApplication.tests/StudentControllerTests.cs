using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentApplication.Controllers;
using StudentApplication.Data;
using StudentApplication.Models;
using StudentApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentApplication.Tests
{
    public class StudentControllerTests
    {
        private readonly StudentController _controller;
        private readonly ApplicationDbContext _context;

        public StudentControllerTests()
        {
            // Set up the in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("StudentsDB")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new StudentController(_context);
        }

        // Cleanup the database after each test
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnAllStudents()
        {
            // Arrange
            _context.Students.Add(new Student
            {
                FirstName = "John",
                LastName = "Doe",
                StudentEmail = "johndoe@gmail.com",
                Phone = "0776747837",
                Address = "123 Main St",
                Country = "USA",
                Institute = "University",
                Intake = "2024",
                CourseTitle = "Computer Science",
                FilePath = "path/to/mockfile.txt"
            });

            _context.Students.Add(new Student
            {
                FirstName = "Jane",
                LastName = "Doe",
                StudentEmail = "janedoe@gmail.com",
                Phone = "0777123456",
                Address = "456 Main St",
                Country = "UK",
                Institute = "Cambridge",
                Intake = "2023",
                CourseTitle = "Engineering",
                FilePath = "path/to/mockfile.txt"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Cast to OkObjectResult
            var students = okResult.Value as List<Student>;
            students.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddStudent_WithValidData_ShouldReturnAddedStudent()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("testfile.txt");
            mockFile.Setup(f => f.Length).Returns(1024); // File size in bytes
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            // Arrange
            var studentDto = new AddStudentDto
            {
                FirstName = "John",
                LastName = "Doe",
                StudentEmail = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                Country = "USA",
                Institute = "University",
                Intake = "2024",
                CourseTitle = "Computer Science",
                File = mockFile.Object
            };

            // Act
            var result = await _controller.AddStudent(studentDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var student = result.Value as Student;
            student.Should().NotBeNull();
            student.FirstName.Should().Be("John");
            student.StudentEmail.Should().Be("john@example.com");
        }
    }



}
