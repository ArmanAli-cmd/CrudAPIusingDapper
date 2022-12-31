using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApplicationUsingDapper.Domain.CourseAggregate;
using WebApplicationUsingDapper.Domain.StudentAggregate;
using WebApplicationUsingDapper.Model;

namespace WebApplicationUsingDapper.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentCourseController : ControllerBase
    {
        public readonly IConfiguration _configure;
        public StudentCourseController(IConfiguration configure)
        {
            _configure = configure;
        }

        [HttpPost("CourseId,StudentId")]
        public async Task<IActionResult> EnrolledCourse(int CourseId, int StudentId)
        {
            using var connection = new SqlConnection(_configure.GetConnectionString("DefaultConnection"));
            var student = await connection.QueryFirstAsync<Student>("SELECT * from Student where StudentId = @id;", new { id = StudentId });
            //if(student==null) return NotFound("Student Not Found");

            var course = await connection.QueryFirstAsync<Course>("SELECT * from Course where CourseId = @id;", new { id = CourseId });
            //if (course == null) return NotFound("Course Not Found");

            DateTime now = DateTime.Now;

            var courseEnrolled = await connection.ExecuteAsync("Insert Into StudentCourse (StudentId, CourseId, Price, DateTime) values(@sId, @cId, @price, @dTime)", new {sId = StudentId, cId = CourseId, price = course.Price, dTime =  now});

            return Ok(courseEnrolled);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllEnrolledStudent()
        {
            using var connection = new SqlConnection(_configure.GetConnectionString("DefaultConnection"));
            var students = await connection.QueryAsync<StudentCourse>("Select * from StudentCourse");
            return Ok(students);
        }

        [HttpGet("StudentId")]
        public async Task<IActionResult> GetAllEnrolledStudents(int StudentId)
        {
            using var connection = new SqlConnection(_configure.GetConnectionString("DefaultConnection"));
            var courses = await connection.QueryAsync<StudentResponseModel>("SELECT StudentCourse.StudentId, Student.StudentName, Course.CourseName, Course.Price, StudentCourse.DateTime FROM(( StudentCourse Inner join Student on Student.StudentId= StudentCourse.StudentId)INNER JOIN Course ON Course.CourseId = StudentCourse.CourseId) where StudentCourse.StudentId=@sId;", new {sId = StudentId});

            return Ok(courses);
        }

        [HttpGet("CourseId")]
        public async Task<IActionResult> GetAllStudentsEnrolled(int CourseId)
        {
            using var connection = new SqlConnection(_configure.GetConnectionString("DefaultConnection"));
            var students = await connection.QueryAsync<StudentResponseModel>("SELECT StudentCourse.StudentId, Student.StudentName, Course.CourseName, Course.Price, StudentCourse.DateTime FROM(( StudentCourse Inner join Student on Student.StudentId= StudentCourse.StudentId)INNER JOIN Course ON Course.CourseId = StudentCourse.CourseId) where StudentCourse.CourseId=@cId;", new { cId = CourseId });
            
            return Ok(students);
        }

        [HttpDelete("StudentId, CourseId")]
        public async Task<IActionResult> DeleteEnrolledCourse(int StudentId, int CourseId)
        {
            using var connection = new SqlConnection(_configure.GetConnectionString("DefaultConnection"));
            var student = await connection.ExecuteAsync("Delete from StudentCourse where StudentId=@sId and CourseId =@cId", new { sId = StudentId , cId = CourseId});
            return Ok(student);
        }
    }
}
