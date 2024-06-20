using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace Sudoku
{
    public class SudokuConstructModel
    {
        private class Row 
        {
            public Row(int header, int length) 
            {
                _Numbers = new();

                for(int i = 0 - header; i <= length - 1 - header; i++) 
                {
                    var num = (i >= 0 ? i : i + length) + 1;

                    _Numbers.Add(num);
                }
            }

            private List<int> _Numbers;

            public int this[int index] 
                => Mathf.Clamp(index, 0, _Numbers.Count - 1) == index ? _Numbers[index] : 0;

            public int First => _Numbers.First();
        }

        public SudokuConstructModel(SudokuMetrix metrix) 
        {
            Metrix = metrix;
        }

        public SudokuMetrix Metrix { get; }

        public void Construct(int size) 
        {
            var length = (int)Mathf.Pow(size, 2);
            var rows   = new List<Row>();

            for(int i = 0; i < length; i++)
            {
                rows.Add(new(i, length));
            }

            rows.Sort((r1, r2) => (r1.First % size).CompareTo(r2.First % size));

            var arrayX = Declarations.EvenlyDistributed(0, size, size);
            var arrayY = Declarations.EvenlyDistributed(0, size, size);
            
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    Metrix[x + y * length]
                        .To<IReposit>()
                        .Preserve(rows[arrayX[x]][arrayY[y]]);
                }
            }
        }
    }
}