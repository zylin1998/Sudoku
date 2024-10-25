using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using Loyufei.ViewManagement;
using Zenject;

namespace Loyufei.MVP
{
    public class ViewPresenter : Presenter
    {
        [Inject]
        protected void Construct(MonoViewBase view, ViewManager manager)
        {
            View    = view;
            Manager = manager;

            Manager.Register(view);

            Init();
        }

        public MonoViewBase View    { get; private set; }
        public ViewManager  Manager { get; private set; }

        protected virtual void Init()
        {

        }
    }

    public class ViewPresenter<TView> : Presenter where TView : MonoViewBase
    {
        [Inject]
        protected void Construct(TView view, ViewManager manager)
        {
            View    = view;
            Manager = manager;

            Manager.Register(view);

            Init();
        }

        public TView       View    { get; private set; }
        public ViewManager Manager { get; private set; }

        protected virtual void Init() 
        {

        }
    }
}
