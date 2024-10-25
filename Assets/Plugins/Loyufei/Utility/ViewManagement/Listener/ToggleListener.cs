using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Loyufei.ViewManagement
{
    public class ToggleListener : MonoListenerAdapter<Toggle>
    {
        public bool IsOn => Listener.isOn;

        public void SetValue(bool isOn, bool withoutNotify = true) 
        {
            if (withoutNotify) { Listener.SetIsOnWithoutNotify(isOn); }

            else { Listener.isOn = isOn; }
        }

        public override void AddListener(Action<IListenerAdapter> callBack)
        {
            Listener.onValueChanged.AddListener((isOn) => callBack.Invoke(this));
        }
    }
}