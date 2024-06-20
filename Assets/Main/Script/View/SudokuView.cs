using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;
using System.Linq;
using UnityEngine.UI;

namespace Sudoku
{
    public class SudokuView : MenuBase
    {
        [SerializeField]
        private Transform       _Sudoku;
        [SerializeField]
        private Transform       _Review;
        [SerializeField]
        private GridLayoutGroup _LayoutGroup;

        [Inject]
        private AreaPool   _AreaPool;
        [Inject]
        private ReviewPool _ReviewPool;

        private List<NumberListener> _Numbers = new();

        public IEnumerable<NumberListener> Numbers => _Numbers;
        public Stack<AreaView>             Areas   { get; } = new();
        public Stack<ReviewListener>       Reviews { get; } = new();

        public void Layout(int size) 
        {
            GridSetting(size);

            LayoutNumbers(size);

            LayoutReviews(size);
        }

        public void Remove()
        {
            RemoveNumbers();
            RemoveReviews();
        }

        public void GridSetting(int size) 
        {
            var rect  = _Sudoku.parent.To<RectTransform>().rect;
            var space = (rect.width - 40) / (size * 10 + size - 1);
            var side  = space * 10;

            _LayoutGroup.cellSize = new Vector2(side, side);
            _LayoutGroup.spacing  = new Vector2(space, space);
            _LayoutGroup.constraintCount = size;
        }

        public IEnumerable<NumberListener> LayoutNumbers(int size) 
        {
            var areaCount = (int)Mathf.Pow(size, 2);

            for(int id = 0; id < areaCount; id++) 
            {
                var area = _AreaPool.Spawn(size, id);

                area.transform.SetParent(_Sudoku);
                area.transform.To<RectTransform>().sizeDelta = _LayoutGroup.cellSize;
                area.GridSetting(size);

                _Numbers.AddRange(area.Numbers);

                Areas.Push(area);
            }

            _Numbers.Sort((n1, n2) => n1.Id.CompareTo(n2.Id));

            return _Numbers;
        }

        public IEnumerable<ReviewListener> LayoutReviews(int size)
        {
            var max = (int)Mathf.Pow(size, 2);

            for (int id = 1; id <= max; id++)
            {
                var review = _ReviewPool.Spawn(id);

                review.transform.SetParent(_Review);

                Reviews.Push(review);
            }

            return Reviews;
        }

        public void RemoveNumbers() 
        {
            _Numbers.Clear();

            for(; Areas.Any();) 
            {
                _AreaPool.Despawn(Areas.Pop());
            }
        }

        public void RemoveReviews()
        {
            for (; Reviews.Any();)
            {
                _ReviewPool.Despawn(Reviews.Pop());
            }
        }
    }

    public class NumberListenerEntity : Entity<int, NumberListener>
    {
        public NumberListenerEntity(int identity, NumberListener listener) 
            : base(identity, listener)
        {

        }
    }
}