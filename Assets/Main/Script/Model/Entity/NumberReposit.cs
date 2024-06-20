using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace Sudoku
{
    public class NumberReposit : RepositBase<int, int>
    {
        public NumberReposit(int id) : base(id, 0) 
        {

        }
    }
}