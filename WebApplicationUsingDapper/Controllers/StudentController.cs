using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using WebApplicationUsingDapper.Domain.CourseAggregate;
using WebApplicationUsingDapper.Domain.StudentAggregate;
using WebApplicationUsingDapper.Model;

namespace WebApplicationUsingDapper.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : ControllerBase
    {
        public readonly IConfiguration _config;
        public StudentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var students = await connection.QueryAsync<Student>("SELECT * from student");

            foreach (var student in students)
            {
                int id = student.StudentId;
                var courses = await connection.QueryAsync<String>("select CourseName from Course where CourseId in (SELECT CourseId from StudentCourse where StudentId = @Sid);", new { Sid = id });
                
                student.Courses = courses.ToList();
            }
            return Ok(students);
        }

        [HttpGet("StudentId")]
        public async Task<IActionResult> GetCourseById(int StudentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.QueryFirstAsync<Student>("SELECT * from Student where StudentId = @id;", new { id = StudentId });
            var courses = await connection.QueryAsync<String>("select CourseName from Course where CourseId in (SELECT CourseId from StudentCourse where StudentId = @Sid);", new { Sid = StudentId });
            
            student.Courses = courses.ToList();
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(StudentRequestModel model)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.ExecuteAsync("Insert into Student (StudentName, StudentHome) values(@name, @home)", new { name = model.StudentName, home = model.StudentHome });
            return Ok(student);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int StudentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.ExecuteAsync("Delete from Student where StudentId=@id", new { id = StudentId });
            return Ok(student);
        }
    }
}
