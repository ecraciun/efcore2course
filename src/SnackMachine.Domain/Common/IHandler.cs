using System;
using System.Collections.Generic;
using System.Text;

namespace SnackMachine.Domain
{
    public interface IHandler<T>
        where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
