using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using Loyufei;

namespace Sudoku
{
    public class SudokuModel
    {
        [Inject]
        public SudokuMetrix  Metrix { get; }
        [Inject]
        public PlayerMetrix  Player { get; }
        [Inject]
        public SudokuSetting Setting { get; }

        public void Start() 
        {
            Metrix.Set();
            Player.Set();
        }

        public void FillRandoms() 
        {
            var randoms = Metrix.GetRandom(Setting.Display).ToList();

            randoms.ForEach(r => Player.Set(r.offset, r.num));
        }

        public void FillRandoms(IEnumerable<Offset2DInt> offsets)
        {
            foreach (var offset in offsets) 
            {
                Player.Set(offset, Metrix.Get(offset));
            }
        }

        public int SetNumber(Offset2DInt offset, int number) 
        {
            Player.Set(offset, number);

            return Player.CheckSame(offset).Count();
        }

        public void Clear(Offset2DInt offset)
        {
            Player.Clear(offset);
        }

        public bool GameOver() 
        {
            for (int index = 0; index < Setting.Capacity; index++)
            {
                if (Metrix.Get(index).number != Player.Get(index).number) { return false; }
            }

            return true;
        }
    }
}
