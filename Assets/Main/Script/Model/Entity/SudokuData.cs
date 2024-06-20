using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Sudoku
{
    public class SudokuSetup : DomainEventBase
    {
        public SudokuSetup(int size, int display, int tips)
        {
            Size    = size;
            Display = display;
            Tips    = tips;
        }

        public int Size    { get; }
        public int Display { get; }
        public int Tips    { get; }

    }
}