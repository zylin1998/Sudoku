using Zenject;
using NUnit.Framework;
using UnityEngine;

namespace Loyufei.DomainEvents.UnitTest
{
    [TestFixture]
    public class DomainEventBusTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Binding()
        {
            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsSingle();

            Container
                .Bind<AggregateRoot>()
                .AsSingle();
        }

        [Test]
        public void EventBusTest() 
        {
            var eventBus  = Container.Resolve<IDomainEventBus>();
            var testEvent = new UnitTestEvent(999);
            var unitTest  = "UnitTest";
            var root      = Container.Resolve<AggregateRoot>();

            eventBus.Register<UnitTestEvent>(UnitTesting, unitTest);

            root.SettleEvents(unitTest, testEvent);

            var unregister = eventBus.UnRegister<UnitTestEvent>(UnitTesting, unitTest);

            Assert.AreEqual(true, unregister);
        }

        private void UnitTesting(UnitTestEvent test) 
        {
            Assert.AreEqual(999, test.TestData);
        }

        private class UnitTestEvent : DomainEventBase 
        {
            public UnitTestEvent(int testData)
            {
                TestData = testData;
            }

            public int TestData { get; }
        }
    }
}