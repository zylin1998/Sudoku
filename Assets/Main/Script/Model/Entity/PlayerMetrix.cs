using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei;

namespace Sudoku
{
    public class PlayerMetrix : RepositoryBase<int, int>
    {
        public PlayerMetrix() : base(CreateReposits(Declarations.Sizes.Max()))
        {

        }

        public PlayerMetrix(IEnumerable<RepositBase<int, int>> entities) : base(entities)
        {

        }

        public int Size { get; private set; }

        public IReposit<int> this[int x, int y]
            => IsClamp(x, y) ? SearchAt(x + y * Size.Pow(2)) : default;

        private bool IsClamp(int x, int y)
        {
            var length = Size.Pow(2);

            return x.IsClamp(0, length) && y.IsClamp(0, length);
        }

        private static IEnumerable<RepositBase<int, int>> CreateReposits(int size)
        {
            var length = size.Pow(4);

            for (int id = 0; id <= length; id++)
            {
                yield return new(id, 0);
            }
        }

        public void Set(int size) 
        {
            Size = size;

            _Reposits.ForEach(r => r.Preserve(0));
        }

        public void Set(Offset2DInt offset, int number) 
        {
            this[offset.X, offset.Y].Preserve(number);
        }

        public IEnumerable<Offset2DInt> CheckHorizontal(Offset2DInt offset) 
        {
            for (var index =  0; index < Size.Pow(2); index++) 
            {
                var target = new Offset2DInt(index, offset.Y);

                if (target == offset) { continue; }

                if (this[target.X, target.Y].Data == this[offset.X, offset.Y].Data) 
                {
                    yield return target;
                }
            }
        }

        public IEnumerable<Offset2DInt> CheckVertical(Offset2DInt offset)
        {
            for (var index = 0; index < Size.Pow(2); index++)
            {
                var target = new Offset2DInt(offset.X, index);

                if (target == offset) { continue; }

                if (this[target.X, target.Y].Data == this[offset.X, offset.Y].Data)
                {
                    yield return target;
                }
            }
        }

        public IEnumerable<Offset2DInt> CheckArea(Offset2DInt offset, bool ignoreHoriandVert = true) 
        {
            var area  = new Offset2DInt(offset.X / Size, offset.Y / Size);
            var start = new Offset2DInt(area.X * Size, area.Y * Size);
            var end   = new Offset2DInt(area.X * Size + Size, area.Y * Size + Size);

            for (var x = start.X; x < end.X; x++) 
            {
                for (var y = start.Y; y < end.Y; y++)
                {
                    var target = new Offset2DInt(x, y);

                    if (target == offset) { continue; }

                    if (ignoreHoriandVert && (x == offset.X || y == offset.Y)) { continue; }

                    if (this[target.X, target.Y].Data == this[offset.X, offset.Y].Data)
                    {
                        yield return target;
                    }
                }
            }
        }

        public IEnumerable<Offset2DInt> GetOffsetsByNumber(int number)
        {
            var (x, y) = (0, 0);

            for (var index = 0; index < Size.Pow(4); index++) 
            {
                var reposit = this[x, y];

                if (reposit.Data == number) yield return new(x, y);

                (x, y) = x < Size.Pow(2) - 1 ? (++x, y) : (0, ++y);
            }
        }

        public Offset2DInt GetRandom() 
        {
            var array  = _Reposits.GetRange(0, Size.Pow(4)).FindAll(r => r.Data == 0);
            var random = Declarations.GetRandom(0, array.Count - 1);
            var index  = _Reposits.IndexOf(array[random]);
            
            return GetOffset(index, Size.Pow(2));
        }

        public void Clear(Offset2DInt offset) 
        {
            this[offset.X, offset.Y]?.Preserve(0);
        }

        private Offset2DInt GetOffset(int number, int size) 
        {
            return new(number % size, number / size);
        }
    }
}
