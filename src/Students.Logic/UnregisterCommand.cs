namespace Students.Logic
{
    public sealed class UnregisterCommand : ICommand
    {
        public long Id { get; }

        public UnregisterCommand(long id)
        {
            Id = id;
        }
    }
}