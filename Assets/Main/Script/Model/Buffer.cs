using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Zenject;
using UnityEngine;

namespace Sudoku
{
    public class Buffer
    {
        [Inject]
        public SudokuMetrix Sudoku { get; }
        [Inject]
        public PlayerMetrix Player { get; }
        
        public (Offset2DInt offset, int number) Get(int index) 
        {
            return Player.Get(index);
        }

        public int Get(Offset2DInt offset)
        {
            return Player[offset.X, offset.Y].Data;
        }

        public IEnumerable<(Offset2DInt offset, int number)> GetAll(bool isAnswer) 
        {
            return isAnswer ? Sudoku.GetAll() : Player.GetAll();
        }

        public IEnumerable<Offset2DInt> GetEmpty(int length) 
        {
            var empties = Player.GetAll().Where(i => i.number <= 0).ToList();
            var randoms = Declarations.RandomList(0, empties.Count, length);

            foreach (var i in randoms) 
            {
                yield return empties[i].offset;
            }
        }

        public bool Verified() 
        {
            var result = true;
            for(var i = 0; i < Player.Setting.Capacity; i++) 
            {
                var offset = Player.Get(i).offset;
                var num1   = Player.Get(i).number;
                var num2   = Sudoku.Get(i).number;

                if (num1 == 0) { continue; }

                if (num1 != num2) 
                {
                    Debug.Log(string.Format("Offset:{0} Sudoku:{1} Player:{2}", offset, num2, num1));
                    
                    result = false; 
                }
            }

            return result;
        }
    }
}