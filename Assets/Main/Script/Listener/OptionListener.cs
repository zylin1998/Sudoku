using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class OptionListener : ButtonListener
    {
        [SerializeField]
        private TextMeshProUGUI _OptionText;

        public TextMeshProUGUI OptionText => _OptionText;

        public void SetText(string text) 
        {
            OptionText.SetText(text);
        }
    }
}