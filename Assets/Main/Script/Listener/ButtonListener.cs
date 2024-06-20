using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku
{
    public class ButtonListener : ListenerAdapter<Button>
    {
        public override void AddListener(Action<object> callBack)
        {
            Listener.onClick.AddListener(() => callBack?.Invoke(Id));
        }
    }
}