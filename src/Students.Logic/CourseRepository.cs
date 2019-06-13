using System.Linq;

namespace Students.Logic
{
    public class CourseRepository
    {
        private readonly StudentContext _context;

        public CourseRepository(StudentContext context)
        {
            _context = context;
        }

        public Course GetByName(string name)
        {
            return _context.Courses.SingleOrDefault(x => x.Name == name);
        }
    }
}