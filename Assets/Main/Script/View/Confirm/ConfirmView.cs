using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Loyufei.ViewManagement;

namespace Sudoku
{
    public class ConfirmView : MonoViewBase
    {
        [SerializeField]
        private TextMeshProUGUI _Message;

        public override object ViewId => Declarations.Message;

        public void SetText(string text) 
        {
            _Message.SetText(text);
        }
    }
}