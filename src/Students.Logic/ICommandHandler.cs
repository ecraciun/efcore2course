namespace Students.Logic
{
    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}