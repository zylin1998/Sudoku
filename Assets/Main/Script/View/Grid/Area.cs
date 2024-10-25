using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Loyufei;

namespace Sudoku
{
    public class Area : MonoBehaviour, IEnumerable<NumberListener>
    {
        [SerializeField]
        private GridLayoutGroup _LayoutGroup;
        [SerializeField]
        private Transform       _Content;

        public Transform       Content     => _Content;
        public GridLayoutGroup LayoutGroup => _LayoutGroup;

        public Stack<NumberListener> Numbers { get; } = new();

        public IEnumerator<NumberListener> GetEnumerator()
        {
            return ((IEnumerable<NumberListener>)Numbers).GetEnumerator();
        }

        public void GridSetting(int size)
        {
            var rect  = transform.To<RectTransform>().rect;
            var space = rect.width / (size * 10 + size - 1);
            var side  = space * 10;

            _LayoutGroup.cellSize = new Vector2(side, side);
            _LayoutGroup.spacing  = new Vector2(space, space);
            _LayoutGroup.constraintCount = size;

            Numbers.ForEach(number =>
                number.transform.To<RectTransform>().sizeDelta = LayoutGroup.cellSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Numbers.GetEnumerator();
        }
    }

    public class AreaPool : MemoryPool<int, int, Area>
    {
        public AreaPool() : base() 
        {
            DespawnRoot = new GameObject(typeof(AreaPool).Name).transform;
        }

        [Inject]
        public NumberPool NumberPool { get; }

        public Transform DespawnRoot { get; }

        protected override void Reinitialize(int size, int id, Area area)
        {
            area.GridSetting(size);

            var length = (int)Mathf.Pow(size, 2);

            var (x, y) = (0, 0);
            for (int i = 0; i < length; i++) 
            {
                var number = NumberPool.Spawn(new Offset2DInt(x, y) + InttoOffset(id, size));
                
                number.transform.SetParent(area.Content);
                number.transform.localScale = Vector3.one;

                area.Numbers.Push(number);

                (x, y) = x < size - 1 ? (++x, y) : (0, ++y);
            }

            area.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Area area)
        {
            var numbers = area.Numbers;

            for(; numbers.Any();) 
            {
                NumberPool.Despawn(numbers.Pop());
            }

            area.gameObject.SetActive(false);

            area.transform.SetParent(DespawnRoot);
        }

        private Offset2DInt InttoOffset(int num, int size) 
        {
            return new(num % size * size, num / size * size);
        }
    }
}