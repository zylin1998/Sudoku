using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Loyufei;

namespace Sudoku
{
    public class OptionListener : ListenerAdapter<Button>
    {
        [SerializeField]
        private TextMeshProUGUI _OptionText;

        public TextMeshProUGUI OptionText => _OptionText;

        public override void AddListener(Action<object> callBack)
        {
            Listener.onClick.AddListener(() => callBack.Invoke(Id));
        }

        public void SetText(string text) 
        {
            OptionText.SetText(text);
        }
    }
}