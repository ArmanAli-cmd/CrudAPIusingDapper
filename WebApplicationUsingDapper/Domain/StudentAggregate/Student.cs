using System.Numerics;
using WebApplicationUsingDapper.Domain.CourseAggregate;

namespace WebApplicationUsingDapper.Domain.StudentAggregate
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentHome { get; set; }
        public List<String> Courses { get; set; }
    }
}
