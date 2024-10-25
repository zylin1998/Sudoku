using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace Loyufei.ViewManagement
{
    public class DropdownListener : MonoListenerAdapter<TMP_Dropdown>
    {
        public int Value => Listener.value;

        public void SetValue(int value, bool withoutNotify = true) 
        {
            if (withoutNotify) { Listener.SetValueWithoutNotify(value); }

            else { Listener.value = value; }
        }

        public void SetOptions(IEnumerable<TMP_Dropdown.OptionData> options) 
        {
            var list = options.ToList();

            Listener.options.Clear();

            Listener.AddOptions(list);
        }

        public override void AddListener(Action<IListenerAdapter> callBack)
        {
            Listener.onValueChanged.AddListener(value => callBack.Invoke(this));
        }
    }
}