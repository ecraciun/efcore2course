using Students.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Students.Logic
{
    public class Student : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        //private readonly IList<Enrollment> _enrollments = new List<Enrollment>();
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public Enrollment FirstEnrollment => GetEnrollment(0);
        public Enrollment SecondEnrollment => GetEnrollment(1);

        //private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
        public List<Disenrollment> Disenrollments { get; set; } = new List<Disenrollment>();

        protected Student()
        {
        }

        public Student(string name, string email)
            : this()
        {
            Name = name;
            Email = email;
        }

        public Enrollment GetEnrollment(int index)
        {
            if (Enrollments.Count > index)
                return Enrollments[index];

            return null;
        }

        public void RemoveEnrollment(Enrollment enrollment, string comment)
        {
            Enrollments.Remove(enrollment);
            var disenrollment = new Disenrollment(enrollment.Student, enrollment.Course, comment);
            Disenrollments.Add(disenrollment);
        }

        public void Enroll(Course course, Grade grade)
        {
            if (Enrollments.Count >= 2)
                throw new Exception("Cannot have more than 2 enrollments");

            var enrollment = new Enrollment(this, course, grade);
            Enrollments.Add(enrollment);
        }
    }
}