using System;

namespace Students.Logic
{
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public RegisterCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }


        public Result Handle(RegisterCommand command)
        {
            var student = new Student(command.Name, command.Email);

            if (command.Course1Id != default && command.Course1Grade != null)
            {
                Course course = _courseRepository.GetById(command.Course1Id);
                student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
            }

            if (command.Course2Id != default && command.Course2Grade != null)
            {
                Course course = _courseRepository.GetById(command.Course2Id);
                student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
            }

            _studentRepository.Add(student);

            return Result.Success();
        }
    }
}