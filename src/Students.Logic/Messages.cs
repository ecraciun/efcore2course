using System;

namespace Students.Logic
{
    public sealed class Messages
    {
        private readonly IServiceProvider _serviceProvider;

        public Messages(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Result Dispatch(ICommand command)
        {
            var type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            Result result = handler.Handle((dynamic)command);
            return result;
        }
    }
}