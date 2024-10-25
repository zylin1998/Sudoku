using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Sudoku
{
    public class SudokuMetrix : RepositoryBase<int, int>
    {
        public SudokuMetrix() : base(CreateReposits(Declarations.Sizes.Max()))
        {

        }

        public SudokuMetrix(IEnumerable<RepositBase<int, int>> entities) : base(entities)
        {

        }

        public int Size { get; private set; }

        public IReposit<int> this[int x, int y] 
            => IsClamp(x, y) ? SearchAt(x + y * Size) : default;

        private bool IsClamp(int x, int y) 
        {
            var length = Size.Pow(2) - 1;

            return x.IsClamp(0, length) && y.IsClamp(0, length);
        }

        private static IEnumerable<RepositBase<int, int>> CreateReposits(int size) 
        {
            var length = size.Pow(4);

            for(int id = 0; id <= length; id++) 
            {
                yield return new(id, 0);
            }
        }

        public void Construct(int size)
        {
            Size = size;

            var rows   = GetRows(Size.Pow(2)).OrderBy(r => r.First % Size).ToArray();
            var arrayX = Declarations.EvenlyDistributed(0, Size, Size);
            var arrayY = Declarations.EvenlyDistributed(0, Size, Size);

            for (int index = 0; index < Size.Pow(4); index++) 
            {
                var offset = GetOffset(index, Size.Pow(2));
                var x      = arrayX[offset.X];
                var y      = arrayY[offset.Y];
                
                _Reposits[index].Preserve(rows[x][y]);
            }
        }

        public int Get(Offset2DInt offset) 
        {
            var reposit = this[offset.X, offset.Y] ?? new RepositBase<int, int>(0, 0);

            return reposit.Data;
        }

        public IEnumerable<(Offset2DInt offset, int num)> GetRandom(int length)
        {
            var random = Declarations.RandomList(0, Size.Pow(4), length);

            for (int index = 0; index < Size.Pow(4); index++)
            {
                if (random.Remove(index)) 
                {
                    yield return (GetOffset(index, Size.Pow(2)), _Reposits[index].Data);
                }
            }
        }

        public IEnumerable<(Offset2DInt offset, int number)> GetAll()
        {
            for (int index = 0; index < Size.Pow(4); index++)
            {
                //Debug.Log(GetOffset(index, Size.Pow(2)));
                yield return (GetOffset(index, Size.Pow(2)), _Reposits[index].Data);
            }
        }

        private IEnumerable<Row> GetRows(int length) 
        {
            for (int i = 0; i < length; i++)
            {
                yield return new(i, length);
            }
        }

        private Offset2DInt GetOffset(int num, int length)
        {
            return new(num % length, num / length);
        }
    }

    public struct Row
    {
        public Row(int header, int length)
        {
            _Numbers = new();

            for (int i = 0 - header; i <= length - 1 - header; i++)
            {
                var num = (i >= 0 ? i : i + length) + 1;

                _Numbers.Add(num);
            }
        }

        private List<int> _Numbers;

        public int this[int index]
            => index.Clamp(0, _Numbers.Count - 1) == index ? _Numbers[index] : 0;

        public int First => _Numbers.First();
    }
}