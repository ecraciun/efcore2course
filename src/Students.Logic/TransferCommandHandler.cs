using System;

namespace Students.Logic
{
    public sealed class TransferCommandHandler : ICommandHandler<TransferCommand>
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public TransferCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }


        public Result Handle(TransferCommand command)
        {
            Student student = _studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found with Id '{command.Id}'");

            Course course = _courseRepository.GetById(command.CourseId);
            if (course == null)
                return Result.Fail($"Course is incorrect: '{command.CourseId}'");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

            enrollment.Update(course, command.Grade);
            _studentRepository.Update(student);

            return Result.Success();
        }
    }
}