using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sudoku
{
    public class MessageView : MenuBase
    {
        [SerializeField]
        private TextMeshProUGUI _Message;

        public void SetText(string text) 
        {
            _Message.SetText(text);
        }

        public override Tween Open()
        {
            gameObject.SetActive(true);

            return base.Open();
        }

        public override Tween Close()
        {
            return base.Close().OnComplete(() => gameObject.SetActive(false));
        }
    }
}