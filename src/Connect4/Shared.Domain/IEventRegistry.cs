using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain
{
    public interface IEventRegistry<TEventBase> where TEventBase : DomainEvent
    {
        void RegisterEvent(TEventBase @event);
    }
}
