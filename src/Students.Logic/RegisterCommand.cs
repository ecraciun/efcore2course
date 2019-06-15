namespace Students.Logic
{
    public sealed class RegisterCommand : ICommand
    {
        public string Name { get; }
        public string Email { get; }
        public long Course1Id { get; }
        public string Course1Grade { get; }
        public long Course2Id { get; }
        public string Course2Grade { get; }

        public RegisterCommand(string name, string email, long course1Id, string course1Grade, long course2Id, string course2Grade)
        {
            Name = name;
            Email = email;
            Course1Id = course1Id;
            Course1Grade = course1Grade;
            Course2Id = course2Id;
            Course2Grade = course2Grade;
        }
    }
}