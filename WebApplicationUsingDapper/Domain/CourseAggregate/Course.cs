using WebApplicationUsingDapper.Domain.StudentAggregate;

namespace WebApplicationUsingDapper.Domain.CourseAggregate
{
    public class Course
    {
        public int CourseId { get; set; } 
        public string CourseName { get; set;}
        public int Price { get; set; }
        public List<String> Students { get; set; }
    }
}
