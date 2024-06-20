using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    public class MaskView : MenuBase
    {
        [SerializeField]
        private InputBoard _Board;

        private Action<int> _Response = (num) => { };

        public InputBoard Board => _Board;

        public void AskNumber(Action<int> response) 
        {
            _Response = response;

            gameObject.SetActive(true);
        }

        public void ResponseNumber(int number) 
        {
            _Response.Invoke(number);

            _Response = (num) => { };

            gameObject.SetActive(false);
        }
    }
}