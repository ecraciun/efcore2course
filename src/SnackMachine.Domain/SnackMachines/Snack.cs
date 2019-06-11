namespace SnackMachine.Domain
{
    public class Snack : AggregateRoot
    {
        public string Name { get; private set; }

        private Snack()
        { }

        public Snack(string name)
        {
            Name = name;
        }
    }
}