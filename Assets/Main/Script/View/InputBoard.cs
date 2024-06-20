using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Loyufei;
using UnityEngine.EventSystems;

namespace Sudoku
{
    public class InputBoard : MonoBehaviour
    {
        [SerializeField]
        private Transform       _Content;
        [SerializeField]
        private Selectable      _DragArea;
        [SerializeField]
        private GridLayoutGroup _LayoutGroup;

        [Inject]
        private InputPool _InputPool;

        public Stack<InputListener> Inputs { get; } = new();

        private Canvas _Canvas;

        private void Awake()
        {
            _Canvas = FindObjectOfType<Canvas>();

            _DragArea
                .OnDragAsObservable()
                .Subscribe(OnDrag);
        }

        public void GridSetting(int size) 
        {
            _LayoutGroup.constraintCount = size;

            var cellSide = _LayoutGroup.cellSize.x;
            var space    = _LayoutGroup.spacing.x;
            var side     = 5 * 2 + cellSide * size + space * (size - 1);
            
            transform.To<RectTransform>().sizeDelta = new Vector2(side, side + 40);
        }

        public IEnumerable<InputListener> Layout(int size) 
        {
            GridSetting(size);

            var length = (int)Mathf.Pow(size, 2);

            for (int id = 1; id <= length; id++) 
            {
                var input = _InputPool.Spawn(id);

                input.transform.SetParent(_Content);

                Inputs.Push(input);
            }

            return Inputs;
        }

        public void Remove() 
        {
            for (; Inputs.Any();) 
            {
                _InputPool.Despawn(Inputs.Pop());
            }
        }

        private void OnDrag(PointerEventData data) 
        {
            ((RectTransform)transform.parent).anchoredPosition += data.delta / _Canvas.scaleFactor;
        }
    }
}