using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace Sudoku
{
    public class SudokuSetting
    {
        [SerializeField]
        private int _Size    = Declarations.Sizes[0];
        [SerializeField]
        private int _Display = Declarations.Sizes[0].Pow(4) / 10;

        public int Size 
        { 
            get => _Size;

            set => _Size = value.Clamp(Mathf.Min(Declarations.Sizes), Mathf.Max(Declarations.Sizes));
        }

        public int Display 
        {
            get => _Display;

            set => _Display = value.Clamp(MinDisplay, MaxDisplay);
        }

        public int Length     => _Size.Pow(2);
        public int Capacity   => _Size.Pow(4);
        public int Tips       => _Size;
        public int MaxDisplay => (int)(_Size.Pow(4) * 0.4f);
        public int MinDisplay => _Size.Pow(4) / 10;
    }
}