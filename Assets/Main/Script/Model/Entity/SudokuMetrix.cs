using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Zenject;

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

        [Inject]
        public SudokuSetting Setting { get; }

        public IReposit<int> this[int x, int y] 
            => IsClamp(x, y) ? SearchAt(x + y * Setting.Length) : default;

        private bool IsClamp(int x, int y) 
        {
            return x.IsClamp(0, Setting.Length - 1) && y.IsClamp(0, Setting.Length - 1);
        }

        private static IEnumerable<RepositBase<int, int>> CreateReposits(int size) 
        {
            var length = size.Pow(4);

            for(int id = 0; id <= length; id++) 
            {
                yield return new(id, 0);
            }
        }

        public void Set()
        {
            var size   = Setting.Size;
            var length = Setting.Length;
            var rows   = GetRows(length).OrderBy(r => r.First % size).ToArray();
            var arrayX = Declarations.EvenlyDistributed(0, size, size);
            var arrayY = Declarations.EvenlyDistributed(0, size, size);

            for (int index = 0; index < Setting.Capacity; index++) 
            {
                var offset = GetOffset(index);
                var x      = arrayX[offset.X];
                var y      = arrayY[offset.Y];
                
                _Reposits[index].Preserve(rows[x][y]);
            }
        }

        public IEnumerable<(Offset2DInt offset, int num)> GetRandom(int length)
        {
            var capacity = Setting.Capacity;
            var random   = Declarations.RandomList(0, capacity, length);

            for (int index = 0; index < capacity; index++)
            {
                if (random.Remove(index)) 
                {
                    yield return Get(index);
                }
            }
        }

        public int Get(Offset2DInt offset) 
        {
            var reposit = this[offset.X, offset.Y] ?? new RepositBase<int, int>(0, 0);
            
            return reposit.Data;
        }

        public (Offset2DInt offset, int number) Get(int index) 
        {
            var offset = GetOffset(index);

            if (IsClamp(offset.X, offset.Y)) 
            {
                return (GetOffset(index), _Reposits[index].Data);
            }

            return (new(int.MinValue, int.MinValue), int.MinValue);
        }

        public IEnumerable<(Offset2DInt offset, int number)> GetAll()
        {
            for (int index = 0; index < Setting.Capacity; index++)
            {
                yield return Get(index);
            }
        }

        private IEnumerable<Row> GetRows(int length) 
        {
            for (int i = 0; i < length; i++)
            {
                yield return new(i, length);
            }
        }

        private Offset2DInt GetOffset(int num)
        {
            return new(num % Setting.Length, num / Setting.Length);
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