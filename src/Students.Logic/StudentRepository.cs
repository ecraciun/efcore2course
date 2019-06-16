using Microsoft.EntityFrameworkCore;
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
            return _context.Students
                //.Include(x => x.Disenrollments)
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course)
                .FirstOrDefault(x => x.Id == id);
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