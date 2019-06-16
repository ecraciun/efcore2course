using Students.Logic;

namespace Students.Web.Dtos
{
    public class StudentEnrollmentDto
    {
        public long CourseId { get; set; }
        public Grade Grade { get; set; }
    }
}