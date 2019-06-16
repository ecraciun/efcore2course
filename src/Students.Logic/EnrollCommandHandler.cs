using System;

namespace Students.Logic
{
    public sealed class EnrollCommandHandler : ICommandHandler<EnrollCommand>
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public EnrollCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public Result Handle(EnrollCommand command)
        {
            Student student = _studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found with Id '{command.Id}'");

            Course course = _courseRepository.GetById(command.CourseId);
            if (course == null)
                return Result.Fail($"Course is incorrect: '{command.CourseId}'");

            student.Enroll(course, command.Grade);
            _studentRepository.Update(student);

            return Result.Success();
        }
    }
}