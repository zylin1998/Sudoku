using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Loyufei.ViewManagement
{
    public class ButtonListener : MonoListenerAdapter<Button>
    {
        public override int Id { get => base.Id; set => base.Id = value; }

        public override void AddListener(Action<IListenerAdapter> callBack)
        {
            Listener.onClick.AddListener(() => callBack.Invoke(this));
        }
    }
}