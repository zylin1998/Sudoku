using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Sudoku
{
    public class DropdownListener : ListenerAdapter<TMP_Dropdown>
    {
        public override void AddListener(Action<object> callBack)
        {
            Listener.onValueChanged.AddListener((value) => callBack(value));
        }

        public void AddOption(IEnumerable<string> options) 
        {
            Listener.options.Clear();

            Listener.AddOptions(options.Select(o => new TMP_Dropdown.OptionData(o)).ToList());
        }
    }
}