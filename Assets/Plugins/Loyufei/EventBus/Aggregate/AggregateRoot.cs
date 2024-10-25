using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.DomainEvents
{
    public interface IAggregateRoot : IEnumerable<IDomainEvent>
    {
        public IDomainEventBus EventBus { get; }

        public void AddEvent(IEnumerable<IDomainEvent> domainEvents);

        public IEnumerable<IDomainEvent> GetEvents();

        public void ClearEvent();
    }

    public class AggregateRoot : IAggregateRoot
    {
        public IDomainEventBus EventBus { get; private set; }

        protected List<IDomainEvent> Events { get; } = new();

        [Inject]
        protected virtual void Construct(IDomainEventBus eventBus) 
        {
            EventBus = eventBus;
        }

        public void AddEvent(IEnumerable<IDomainEvent> domainEvents)
        {
            Events.AddRange(domainEvents);
        }

        public IEnumerable<IDomainEvent> GetEvents() 
        {
            return Events;
        }

        public void ClearEvent() 
        {
            Events.Clear(); 
        }

        public IEnumerator<IDomainEvent> GetEnumerator()
        {
            return Events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
