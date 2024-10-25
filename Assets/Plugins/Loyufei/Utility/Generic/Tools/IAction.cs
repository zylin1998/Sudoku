using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public interface IAction
    {
        public IAction OnStart   (Action onStart);
        public IAction OnPeriod  (Action onPeriod);
        public IAction OnComplete(Action onComplete);
    }

    public interface IAction<T>
    {
        public IAction<T> OnStart   (Action<T> onStart);
        public IAction<T> OnPeriod  (Action<T> onPeriod);
        public IAction<T> OnComplete(Action<T> onComplete);
    }
}
