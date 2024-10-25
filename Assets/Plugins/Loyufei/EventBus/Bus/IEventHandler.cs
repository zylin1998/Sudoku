using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.DomainEvents
{
    public interface IEventHandler 
    {
        public void Invoke(IDomainEvent eventdata);
    }

    public interface IEventHandler<TEvent> : IEventHandler, IEquatable<Action<TEvent>> where TEvent : IDomainEvent
    {
        public void Invoke(TEvent eventdata);

        void IEventHandler.Invoke(IDomainEvent eventdata) => Invoke(eventdata is TEvent data ? data : default);
    }

    public class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        public EventHandler(Action<TEvent> callBack)
        {
            _CallBack = callBack;
        }
        
        protected Action<TEvent> _CallBack { get; }

        public bool Equals(Action<TEvent> other)
        {
            return Equals(_CallBack, other);
        }

        public virtual void Invoke(TEvent eventData)
        {
            _CallBack?.Invoke(eventData);
        }
    }
}