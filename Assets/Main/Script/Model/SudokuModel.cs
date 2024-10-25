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
        public SudokuMetrix Metrix { get; }
        [Inject]
        public PlayerMetrix Player { get; }
        [Inject]
        public Buffer       Buffer { get; }

        public void Start(int size) 
        {
            Metrix.Construct(size);

            Player.Set(size);
        }

        public void GetRandom(int length) 
        {
            var randoms = Metrix.GetRandom(length).ToList();

            randoms.ForEach(r => Player.Set(r.offset, r.num));
            
            Buffer.SetDisplay(randoms);
        }

        public void GetRandom()
        {
            var offset = Player.GetRandom();
            var number = Metrix.Get(offset);
            
            Player.Set(offset, number);

            Buffer.SetDisplay((offset, number));
        }

        public int SetNumber(Offset2DInt offset, int number) 
        {
            Player.Set(offset, number);

            return Player.CheckArea(offset).Concat(Player.CheckHorizontal(offset)).Concat(Player.CheckVertical(offset)).Count();
        }

        public void Clear(Offset2DInt offset)
        {
            Player.Clear(offset);
        }

        public bool GameOver() 
        {
            var (x, y) = (0, 0);
            for (int index = 0; index < Metrix.Size.Pow(2); index++)
            {
                if (Metrix[x, y].Data != Player[x, y].Data) { return false; }

                (x, y) = x < Metrix.Size.Pow(2) - 1 ? (++x, y) : (0, ++y);
            }

            return true;
        }

        public void QueryAll()
        {
            Buffer.SetDisplay(Metrix.GetAll());
        }
    }
}
