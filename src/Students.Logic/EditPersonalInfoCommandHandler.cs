namespace Students.Logic
{
    public sealed class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
    {
        private readonly StudentRepository _studentRepository;

        public EditPersonalInfoCommandHandler(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Result Handle(EditPersonalInfoCommand command)
        {
            var student = _studentRepository.GetById(command.Id);
            if (student == null)
            {
                return Result.Fail("Student not found");
            }
            student.Name = command.Name;
            student.Email = command.Email;
            _studentRepository.Update(student);
            return Result.Success();
        }
    }
}