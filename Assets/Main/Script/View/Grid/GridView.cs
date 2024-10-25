using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei;
using Loyufei.ViewManagement;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sudoku
{
    public class GridView : MonoViewBase
    {
        public override object ViewId => Declarations.Grid;

        [SerializeField]
        private Transform       _Sudoku;
        [SerializeField]
        private GridLayoutGroup _LayoutGroup;
        [SerializeField]
        private float           _DisplayTime = 2f;

        [Inject]
        private AreaPool _AreaPool;
        
        public List<Area>           Areas   { get; } = new();
        public List<NumberListener> Numbers { get; } = new();
        
        public NumberListener this[int x, int y]
            => Numbers.Find(n => n.Offset.X == x && n.Offset.Y == y);

        public ILayout Layout(int size)
        {
            GridSetting(size);

            LayoutNumbers(size);

            return Layout();
        }

        public override void RemoveLayout()
        {
            Numbers.Clear();

            Areas.ForEach(_AreaPool.Despawn);

            Areas.Clear();
        }

        private void GridSetting(int size)
        {
            var rect  = _Sudoku.parent.To<RectTransform>().rect;
            var space = (rect.width - 40) / (size * 10 + size - 1);
            var side  = space * 10;

            _LayoutGroup.cellSize = new Vector2(side, side);
            _LayoutGroup.spacing  = new Vector2(space, space);
            _LayoutGroup.constraintCount = size;
        }

        private void LayoutNumbers(int size)
        {
            var areaCount = size.Pow(2);

            for (int id = 0; id < areaCount; id++)
            {
                var area = _AreaPool.Spawn(size, id);

                var t = area.transform.To<RectTransform>();

                t.SetParent(_Sudoku);
                t.sizeDelta  = _LayoutGroup.cellSize;
                t.localScale = Vector3.one;

                area.GridSetting(size);

                Areas.Add(area);
                Numbers.AddRange(area.Numbers);
            }

            Numbers.Sort((n1, n2) => OffsetToInt(n1.Offset, areaCount).CompareTo(OffsetToInt(n2.Offset, areaCount)));

            var interval = _DisplayTime / Numbers.Count;
            var count    = 0;

            Observable
                .Interval(TimeSpan.FromSeconds(interval))
                .TakeWhile(l => count < Numbers.Count)
                .Subscribe(
                    (l) => Numbers[count++].gameObject.SetActive(true),
                    ()  => Numbers.ForEach(n => n.Listener.interactable = true));
        }

        public void SetBinder(Action<IListenerAdapter> callBack) 
        {
            _AreaPool.NumberPool.Binder = callBack;
        }

        public void Display(IEnumerable<(Offset2DInt offset, int number)> displays) 
        {
            displays.ToList().ForEach(display =>
            {
                Display(display.offset, display.number);
            });
        }

        public void Display(Offset2DInt offset, int number)
        {
            this[offset.X, offset.Y]?.Interact(number);
        }

        public void Warning(int num, Offset2DInt center) 
        {
            Numbers
                .FindAll(n => n.Context == num)
                .ForEach(number => 
                {
                    number.Warning(number.Offset == center);
                });
        }

        public void Review(int num, bool isOn) 
        {
            Numbers
                .FindAll(n => n.Context == num)
                .ForEach(number =>
                {
                    number.Review(isOn);
                });
        }

        private int OffsetToInt(Offset2DInt offset, int size) 
        {
            return offset.X + offset.Y * size;
        }
    }
}
