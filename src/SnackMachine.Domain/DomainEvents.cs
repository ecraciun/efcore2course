using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SnackMachine.Domain
{
    public static class DomainEvents
    {
        private static List<Type> _handlers;
        private static IServiceProvider ServiceProvider;

        public static void Init(IServiceProvider serviceProvider)
        {
            _handlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
                .ToList();
            ServiceProvider = serviceProvider;
        }

        public static void Dispatch(IDomainEvent domainEvent)
        {
            foreach (Type handlerType in _handlers)
            {
                bool canHandleEvent = handlerType.GetInterfaces()
                    .Any(x => x.IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(IHandler<>)
                        && x.GenericTypeArguments[0] == domainEvent.GetType());

                if (canHandleEvent)
                {
                    dynamic handler = Activator.CreateInstance(handlerType, 
                        new HeadOfficeRepository(ServiceProvider.GetService(typeof(SnackMachineContext)) as SnackMachineContext));
                    handler.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}