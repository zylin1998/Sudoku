using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei.ViewManagement
{
    public interface ILayout
    {
        public void BindListenerAll<TListener>(Action<IListenerAdapter> callBack) where TListener : IListenerAdapter;

        public void BindListener<TListener>(int id, Action<IListenerAdapter> callBack) where TListener : IListenerAdapter;

        public IEnumerable<T> FindAll<T>();

        public T Find<T>(Func<T, bool> match);
    }
}
