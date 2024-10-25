using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Loyufei.DomainEvents
{
    public class DomainEventBus : IDomainEventBus
    {
        public DomainEventBus(DiContainer container) 
        {
            SignalBusInstaller.Install(container);
        }

        protected static EmptyId NullId { get; } = new();

        [Inject]
        public SignalBus SignalBus { get; }

        protected Dictionary<object, EventGroup> Mapping { get; } = new();

        public void Register<TEvent>(IEventHandler<TEvent> eventHandler, object groupId = default, bool priority = false)
            where TEvent : IDomainEvent
        {
            var id = groupId.IsDefault() ? NullId : groupId;

            var group = Mapping.GetorAdd(id, () => CreateGroup(id));
            
            group.Register(eventHandler, priority);
        }

        public bool UnRegister<TEvent>(Action<TEvent> callBack, object groupId = default)
            where TEvent : IDomainEvent
        {
            var id = groupId.IsDefault() ? NullId : groupId;

            var exist = Mapping.TryGetValue(id, out var group);

            return exist ? group.UnRegister(callBack) > 0 : false;
        }

        public void PostAll(IAggregateRoot trigger, object groupId = null)
        {
            var id = groupId.IsDefault() ? NullId : groupId;

            foreach (var e in trigger)
            {
                SignalBus.TryFireId(id, e);
            }

            trigger.ClearEvent();
        }

        protected EventGroup CreateGroup(object groupId) 
        {
            var group = new EventGroup(groupId);

            SignalBus.DeclareSignal<IDomainEvent>(group.GroupId);
            
            SignalBus.SubscribeId<IDomainEvent>(group.GroupId, group.Post);

            return group;
        }

        protected class EventGroup 
        {
            public EventGroup(object groupId) 
            {
                GroupId = groupId;
            }

            public object GroupId { get; }

            public Dictionary<Type, List<IEventHandler>> Handlers { get; } = new();
            
            public void Register<TEvent>(IEventHandler<TEvent> eventHandler, bool priority = false)
                where TEvent : IDomainEvent
            {
                var eventType = typeof(TEvent);

                var handlers = Handlers.GetorAdd(eventType, () => new());

                if (priority) { handlers.Insert(0, eventHandler); }

                else { handlers.Add(eventHandler); }
            }

            public int UnRegister<TEvent>(Action<TEvent> callBack)
                where TEvent : IDomainEvent
            {
                var eventType = typeof(TEvent);
                var exist     = Handlers.TryGetValue(eventType, out var handlers);
                
                return exist ? handlers.RemoveAll(h => ((IEventHandler<TEvent>)h).Equals(callBack)) : 0;
            }

            public void Post(IDomainEvent eventData)
            {
                var actions = Handlers.GetorAdd(eventData.GetType(), () => new());

                actions.ForEach(action => action.Invoke(eventData));
            }
        }

        protected struct EmptyId { }
    }
}