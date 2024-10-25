using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Zenject;

namespace Loyufei
{
    public class KeyUnityInput : BaseInput
    {
        [Inject]
        public InputManager InputManager { get; }

        public override float GetAxisRaw(string axisName)
        {
            return InputManager.Main.GetAxisRaw(axisName);
        }

        public override bool GetButtonDown(string buttonName)
        {
            return InputManager.Main.GetButtonDown(buttonName);
        }
    }
}
