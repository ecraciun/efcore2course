using System.Collections.Generic;
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

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(long id)
        {
            return _context.Courses.Find(id);
        }
    }
}