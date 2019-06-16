namespace Students.Logic
{
    public sealed class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
    {
        private readonly StudentRepository _studentRepository;

        public DisenrollCommandHandler(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Result Handle(DisenrollCommand command)
        {
            Student student = _studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Fail("Disenrollment comment is required");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            _studentRepository.Update(student);

            return Result.Success();
        }
    }
}