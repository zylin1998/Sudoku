using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei;
using Zenject;

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

        [Inject]
        public SudokuSetting Setting { get; }

        public IReposit<int> this[int x, int y]
            => IsClamp(x, y) ? SearchAt(x + y * Setting.Length) : default;

        private bool IsClamp(int x, int y)
        {
            var length = Setting.Length - 1;

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

        public void Set() 
        {
            _Reposits.ForEach(r => r.Preserve(0));
        }

        public void Set(Offset2DInt offset, int number) 
        {
            this[offset.X, offset.Y].Preserve(number);
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

        public IEnumerable<Offset2DInt> CheckSame(Offset2DInt center) 
        {
            if (!IsClamp(center.X, center.Y)) { yield break; }

            var value = this[center.X, center.Y].Data;

            for (var index = 0; index < Setting.Capacity; index++) 
            {
                var offset = GetOffset(index);

                if (Equals(center, offset) || !IsClamp(offset.X, offset.Y)) { continue; }

                if (this[offset.X, offset.Y].Data != value) { continue; }

                if (offset.X == center.X)     { yield return offset; }
                if (offset.Y == center.Y)     { yield return offset; }
                if (SameArea(center, offset)) { yield return offset; }
            }
        }

        public bool SameArea(Offset2DInt offset1, Offset2DInt offset2) 
        {
            var size  = Setting.Size;
            var area1 = new Offset2DInt(offset1.X / size, offset1.Y / size);
            var area2 = new Offset2DInt(offset2.X / size, offset2.Y / size);

            return Equals(area1, area2);
        }

        public void Clear(Offset2DInt offset) 
        {
            this[offset.X, offset.Y]?.Preserve(0);
        }

        private Offset2DInt GetOffset(int number) 
        {
            return new(number % Setting.Length, number / Setting.Length);
        }
    }
}
