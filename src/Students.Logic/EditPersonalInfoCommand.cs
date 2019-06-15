namespace Students.Logic
{
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}