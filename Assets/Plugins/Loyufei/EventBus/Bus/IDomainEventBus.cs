using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.DomainEvents 
{
    public interface IDomainEventBus
    {
        public void Register<TEvent>(IEventHandler<TEvent> eventHandler, object groupId = default, bool priority = false) 
            where TEvent : IDomainEvent;

        public bool UnRegister<TEvent>(Action<TEvent> callBack, object groupId = default)
            where TEvent : IDomainEvent;

        public void PostAll(IAggregateRoot events, object groupId = null);
    }

    public static class DomainEventBusExtensions 
    {
        public static void Register<TEvent>(
            this IDomainEventBus self, 
                 Action<TEvent>  eventAction,
                 object          groupId  = default,
                 bool            priority = false)
            where TEvent : IDomainEvent
        {
            self.Register(new EventHandler<TEvent>(eventAction), groupId, priority);
        }
    }
}