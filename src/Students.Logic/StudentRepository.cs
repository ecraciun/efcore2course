using System.Collections.Generic;
using System.Linq;

namespace Students.Logic
{
    public class StudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public Student GetById(long id)
        {
            return _context.Students.Find(id);
        }

        public IReadOnlyList<Student> GetList(string enrolledIn, int? numberOfCourses)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(enrolledIn))
            {
                query = query.Where(x => x.Enrollments.Any(e => e.Course.Name == enrolledIn));
            }

            List<Student> result = query.ToList();

            if (numberOfCourses != null)
            {
                result = result.Where(x => x.Enrollments.Count == numberOfCourses).ToList();
            }

            return result;
        }

        public void Update(Student student)
        {
            _context.Update(student);
            _context.SaveChanges();
        }

        public void Add(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
        }

        public void Delete(Student student)
        {
            _context.Remove(student);
            _context.SaveChanges();
        }
    }
}