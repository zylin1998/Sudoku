using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using UnityEngine;
using Zenject;

namespace Loyufei.MVP
{
    public class Presenter : AggregateRoot
    {
        public virtual object GroupId { get; }

        protected override void Construct(IDomainEventBus eventBus)
        {
            base.Construct(eventBus);
            
            RegisterEvents();
        }

        protected virtual void RegisterEvents() 
        {

        }

        protected void SettleEvents(params IDomainEvent[] events) 
        {
            this.SettleEvents(GroupId, events);
        }

        protected void Register<TDomainEvent>(Action<TDomainEvent> callBack, bool priority = false) where TDomainEvent : IDomainEvent
        {
            EventBus.Register(callBack, GroupId, priority);
        }
    }
}