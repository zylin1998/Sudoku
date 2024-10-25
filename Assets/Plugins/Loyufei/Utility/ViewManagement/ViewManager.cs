using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.ViewManagement
{
    public enum EShowViewMode
    {
        Single = 0,
        Additive = 1,
    }

    public class ViewManager
    {
        public ViewManager() 
        {
            Views = new();
        }

        public Dictionary<object, IView> Views { get; }

        public IView Current { get; private set; }

        private Stack<IView> Actives { get; } = new();

        public void Register(IView view)
        {
            Views.Add(view.ViewId, view);
        }

        public bool Unregister(object key)
        {
            return Views.Remove(key);
        }

        public IObservable<long> Show(object id) 
        {
            var registered = Views.TryGetValue(id, out var view);

            if (!registered) { return default; }

            Actives.Push(view);

            Current = Actives.Peek();

            return view.Open();
        }

        public IObservable<long> Close() 
        {
            if (Current.IsDefault()) { return default; }

            var temp = Actives.Pop();

            var any = Actives.TryPeek(out var view);

            if (any) { Current = view; }

            return temp.Close();
        }
    }
}