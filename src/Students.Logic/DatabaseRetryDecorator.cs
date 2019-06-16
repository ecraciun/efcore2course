using System;
using System.Collections.Generic;
using System.Text;

namespace Students.Logic
{
    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler)
        {
            _handler = handler;
        }

        public Result Handle(TCommand command)
        {
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    return _handler.Handle(command);
                }
                catch(Exception ex)
                {
                    if (!IsDatabaseException(ex))
                        throw;
                }
            }

            return Result.Fail("Db problems");
        }

        private bool IsDatabaseException(Exception ex)
        {
            string message = ex.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("connection");
        }
    }
}
