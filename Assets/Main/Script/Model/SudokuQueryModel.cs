using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Sudoku
{
    public class SudokuQueryModel
    {
        public SudokuQueryModel(SudokuMetrix metrix) 
        {
            Metrix = metrix;
        }

        public SudokuMetrix Metrix { get; }

        public int Query(int id) 
        {
            return Metrix[id].Data;
        }

        public IEnumerable<IEntity<int>> QueryAll() 
        {
            return Metrix.OfType<NumberReposit>().Where(e => !Equals(e.Data, 0)).ToList();
        }
    }
}