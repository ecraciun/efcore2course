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

        public T Dispatch<T>(IQuery<T> query)
        {
            var type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            T result = handler.Handle((dynamic)query);
            return result;
        }
    }
}