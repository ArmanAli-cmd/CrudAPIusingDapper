using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using WebApplicationUsingDapper.Domain.CourseAggregate;
using WebApplicationUsingDapper.Domain.StudentAggregate;
using WebApplicationUsingDapper.Model;

namespace WebApplicationUsingDapper.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CourseController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var courses = await connection.QueryAsync<Course>("SELECT * from course");
            
            foreach(var course in courses)
            {
                int id = course.CourseId;
                var students = await connection.QueryAsync<String>("select StudentName from student where studentId in (SELECT StudentId from StudentCourse where CourseId = @Cid);", new {Cid = id});
                
                course.Students = students.ToList();
            }

            return Ok(courses);
        }

        [HttpGet("CourseId")]
        public async Task<IActionResult> GetCourseById(int CourseId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var course = await connection.QueryFirstAsync<Course>("SELECT * from course where CourseId = @id;", new {id = CourseId});
         
            var students = await connection.QueryAsync<String>("select StudentName from student where studentId in (SELECT StudentId from StudentCourse where CourseId = @Cid);", new { Cid = CourseId });
            
            course.Students = students.ToList();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseRequestModel model)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var course = await connection.ExecuteAsync("Insert into Course (CourseName, Price) values(@CourseName, @price)", new {CourseName = model.CourseName, Price = model.Price});
            return Ok(course);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int CourseId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var course = await connection.ExecuteAsync("Delete from Course where CourseId=@id", new { id = CourseId });
            return Ok(course);
        }

    }
}
