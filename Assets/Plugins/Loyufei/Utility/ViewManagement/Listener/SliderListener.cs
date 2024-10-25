using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Loyufei.ViewManagement
{
    public class SliderListener : MonoListenerAdapter<Slider>
    {
        public float Value => Listener.value;

        public void SetValue(float value, bool withoutNotify = true)
        {
            if (withoutNotify) { Listener.SetValueWithoutNotify(value); }

            else { Listener.value = value; }
        }

        public override void AddListener(Action<IListenerAdapter> callBack)
        {
            Listener.onValueChanged.AddListener(value => callBack.Invoke(this));
        }
    }
}
