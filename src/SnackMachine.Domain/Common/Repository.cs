namespace SnackMachine.Domain
{
    public abstract class Repository<T>
        where T : AggregateRoot
    {
        protected readonly SnackMachineContext Context;

        public Repository(SnackMachineContext context)
        {
            Context = context;
        }

        public virtual T GetById(long id)
        {
            return Context.Find<T>(id);
        }

        public void Save(T aggregateRoot)
        {
            Context.Update(aggregateRoot);
            Context.SaveChanges();
        }
    }
}