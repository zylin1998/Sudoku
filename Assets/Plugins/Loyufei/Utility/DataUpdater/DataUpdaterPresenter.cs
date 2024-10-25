using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei
{
    public class DataUpdaterPresenter : MVP.Presenter
    {
        public DataUpdaterPresenter(DataUpdater updater)
        {
            Updater = updater;
        }

        public DataUpdater Updater { get; }

        protected override void RegisterEvents()
        {
            Register<UpdateData>(Update);
        }

        public void Update(UpdateData update) 
        {
            Updater.Update(update.Id, update.Value);
        }
    }

    public class UpdateData : DomainEventBase 
    {
        public UpdateData(object id, object value) 
        {
            Id    = id;
            Value = value;
        }

        public object Id    { get; }
        public object Value { get; }
    }
}