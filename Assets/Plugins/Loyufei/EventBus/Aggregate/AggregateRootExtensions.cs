using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.DomainEvents
{
    public static class AggregateRootExtensions
    {
        public static void SettleEvents(this IAggregateRoot self, object identifier = null, params IDomainEvent[] domainEvents) 
        {
            self.AddEvent(domainEvents);

            self.EventBus.PostAll(self, identifier);
        }
    }
}
