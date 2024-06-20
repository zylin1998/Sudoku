using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace Sudoku
{
    public class SudokuMetrix : EntityForm<int, NumberReposit>
    {
        public SudokuMetrix() : base(CreateReposits(Mathf.Max(Declarations.Sizes)))
        {

        }

        public SudokuMetrix(IEnumerable<NumberReposit> entities) : base(entities)
        {

        }

        private static IEnumerable<NumberReposit> CreateReposits(int size) 
        {
            var list = new List<NumberReposit>();

            for(int id = 0; id <= (int)Mathf.Pow(size, 4); id++) 
            {
                list.Add(new(id));
            }

            return list;
        }
    }
}