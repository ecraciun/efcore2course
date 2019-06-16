using Students.Logic;

namespace Students.Web.Dtos
{
    public class StudentTransferDto
    {
        public long CourseId { get; set; }
        public Grade Grade { get; set; }
    }
}